using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Domain;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Chat.Domain.Repository;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Model;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.Repository;
using Renty.Server.Transaction.Domain;
using Renty.Server.Transaction.Domain.Repository;

namespace Renty.Server.Chat.Service
{
    public class ChatService(IMemoryCache memoryCache, IChatRepository chatRepo, IProductRepository productRepo, ITradeOfferRepository tradeOfferRepo)
    {
        private static readonly TimeSpan slidingExpiration = TimeSpan.FromMinutes(30);

        private SemaphoreSlim GetSemaphore(string cacheKey)
        {
            var lazySemaphore = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.SlidingExpiration = slidingExpiration;
                entry.RegisterPostEvictionCallback((cacheKey, value, reason, state) =>
                {
                    // 캐시에서 제거될 때 SemaphoreSlim Dispose
                    if (value is Lazy<SemaphoreSlim> lazy && lazy.IsValueCreated)
                    {
                        lazy.Value.Dispose(); // Dispose 호출
                    }
                });

                return new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1));
            });

            return lazySemaphore!.Value;
        }

        private ChatRooms CreateRoom(int itemId)
        {
            var now = TimeHelper.GetKoreanTime();

            return new()
            {
                ItemId = itemId,
                ChatCount = 0,
                CreatedAt = now,
                UpdatedAt = now,
            };
        }

        private ChatUsers CreateUser(string userId, string roomName)
        {
            var now = TimeHelper.GetKoreanTime();
            return new()
            {
                RoomName = roomName,
                UserId = userId,
                JoinedAt = now,
                LastReadAt = now,
            };
        }

        private TradeOffers CreateTradeOffer(Items item, string buyerId)
        {
            return new()
            {
                Item = item,
                Price = item.Price,
                SecurityDeposit = item.SecurityDeposit,
                PriceUnit = item.PriceUnit,
                BuyerId = buyerId,
                CreatedAt = TimeHelper.GetKoreanTime(),
            };
        }

        private ChatRoomDetailResponse CreateDetailResponse(ChatRooms room, TradeOffers tradeOffer, ChatUsers user)
        {
            var isSeller = tradeOffer.BuyerId != user.UserId;
            var offer = new Offer
            {
                BorrowStartAt = tradeOffer.BorrowStartAt,
                ReturnAt = tradeOffer.ReturnAt,
                Price = tradeOffer.Price,
                SecurityDeposit = tradeOffer.SecurityDeposit,
                PriceUnit = tradeOffer.PriceUnit,
                State = tradeOffer.State,
                Title = tradeOffer.Item.Title,
                ImageUrl = tradeOffer.Item.ItemImages.First().ImageUrl,
            };
            var users = room.ChatUsers.Where(u => u.LeftAt == null)
                .Select(user => new User
                {
                    Name = user.User.UserName!,
                    ProfileImageUrl = user.User.ProfileImage,
                    LastReadAt = user.LastReadAt,
                })
                .ToList();

            var messages = room.ChatUsers.DistinctBy(u => u.UserId)
                .Join(room.Messages, u => u.Id, m => m.SenderId, (u, m) => new { ChatUser = u, Message = m })
                .Where(r => r.Message.CreatedAt > user.JoinedAt)
                .Select(result => new Message
                {
                    SenderName = result.ChatUser.User.UserName!,
                    SendAt = result.Message.CreatedAt,
                    Content = result.Message.Content,
                    Type = result.Message.Type,
                })
                .OrderByDescending(m => m.SendAt)
                .ToList();
            return new ChatRoomDetailResponse
            {
                RoomId = room.Id,
                Offer = offer,
                CallerName = user.User.UserName!,
                IsSeller = isSeller,
                Users = users,
                Messages = messages
            };
        }

        private ChatMessages CreateMessage(ChatUsers user, string content, MessageType type)
        {
            var now = TimeHelper.GetKoreanTime();
            return new()
            {
                Content = content,
                Type = type,
                CreatedAt = now,
                Sender = user, // 이 부분은 나중에 설정해야 함
            };
        }

        private MessageResponse CreateMessageResponse(int roomId, ChatMessages message, string userName)
        {
            return new MessageResponse
            {
                ChatRoomId = roomId,
                SenderId = userName,
                Timestamp = message.CreatedAt,
                Content = message.Content,
                Type = message.Type,
            };
        }

        public async Task<int> CreateItemChatRoom(int itemId, string buyerId, string buyerName)
        {
            string cacheKey = $"ChatRoom_{itemId}_{buyerId}";
            var semaphore = GetSemaphore(cacheKey);

            await semaphore.WaitAsync();
            
            try
            {
                var item = await productRepo.FindBy(itemId) ?? throw new ItemNotFoundException();
                if (item.SellerId == buyerId)
                {
                    throw new SelfChatCreationException();
                }

                var room = await chatRepo.FindByItem(itemId, buyerId);
                var seller = item.Seller;

                if (room == null)
                {
                    var newRoom = CreateRoom(itemId);
                    newRoom.JoinUser(CreateUser(buyerId, seller.UserName!));
                    newRoom.JoinUser(CreateUser(item.SellerId, buyerName));

                    item.AddTradeOffer(CreateTradeOffer(item, buyerId));

                    chatRepo.Add(newRoom);
                    room = newRoom;
                }
                else if (room.ChatUsers.Any(u => u.UserId == buyerId && u.LeftAt == null))
                {
                    throw new ChatRoomAlreadyExistsException() { RoomId = room.Id };
                }
                else 
                {
                    var buyer = room.ChatUsers.FirstOrDefault(u => u.UserId == buyerId);
                    if (buyer == null)
                    {
                        var newUser = CreateUser(buyerId, seller.UserName!);
                        room.JoinUser(newUser);
                    }
                    else
                    {
                        var now = TimeHelper.GetKoreanTime();
                        buyer.LeftAt = null;
                        buyer.JoinedAt = now;
                        buyer.LastReadAt = now;
                    }
                }
                
                await chatRepo.Save();

                return room.Id;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<(string receiverId, MessageResponse message)> SendMessage(int roomId, string callerId, string userName, string content, MessageType type)
        {
            var now = TimeHelper.GetKoreanTime();
            var room = await chatRepo.FindBy(roomId, now)
                ?? throw new ChatRoomNotFoundException();
            var caller = room.ChatUsers.FirstOrDefault(user => user.UserId == callerId && user.LeftAt == null)
                ?? throw new UserNotFoundException();

            var otherUser = room.ChatUsers.First(user => user.UserId != callerId);
            if (otherUser.LeftAt != null)
            {
                otherUser.LeftAt = null;
                otherUser.JoinedAt = now;
                otherUser.LastReadAt = now;
            }

            var message = CreateMessage(caller, content, type);
            room.AddMessage(message);

            await chatRepo.Save();
            return (otherUser.User.Id, CreateMessageResponse(roomId, message, userName));
        }

        public async Task RecordReadTime(int roomId, string userId)
        {
            await chatRepo.UpdateReadAt(roomId, userId, TimeHelper.GetKoreanTime());
        }

        public async Task<ChatRoomDetailResponse> GetRoomDetail(int roomId, string userId, DateTime lastReadAt)
        {
            var room = await chatRepo.FindBy(roomId, lastReadAt) ?? throw new ChatRoomNotFoundException();
            var user = room.ChatUsers.FirstOrDefault(u => u.UserId == userId && u.LeftAt == null) ?? throw new UserNotFoundException();

            var tradeOffer = await tradeOfferRepo.FindBy(room.ItemId, room.ChatUsers.First(u => u.UserId != room.Item.SellerId).UserId);
            user.LastReadAt = TimeHelper.GetKoreanTime();

            var detail = CreateDetailResponse(room, tradeOffer!, user);
            await chatRepo.Save();
            return detail;
        }

        public async Task<ICollection<ChatRoomResponce>> GetUserChatRooms(string userId)
        {
            return await chatRepo.GetRoomList(userId);
        }

        public async Task LeaveRoom(int roomId, string userId)
        {
            var room = await chatRepo.FindBy(roomId, TimeHelper.GetKoreanTime()) ?? throw new ChatRoomNotFoundException();
            var user = room.ChatUsers.FirstOrDefault(u => u.UserId == userId && u.LeftAt == null) ?? throw new UserNotFoundException();

            if (room.ChatUsers.Count(u => u.LeftAt == null) == 1)
            {
                var tradeOffer = await tradeOfferRepo.FindBy(room.ItemId, room.ChatUsers.First(u => u.UserId != room.Item.SellerId).UserId);
                tradeOfferRepo.Remove(tradeOffer!);
                chatRepo.Remove(room);
            }
            else
            {
                user.LeftAt = TimeHelper.GetKoreanTime();
            }

            await chatRepo.Save();
        }
    }
}
