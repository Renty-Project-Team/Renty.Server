using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Domain;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Chat.Domain.Repository;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Product.Domain.Repository;

namespace Renty.Server.Chat.Service
{
    public class ChatRoomService(IMemoryCache memoryCache, IChatRepository chatRepo, IProductRepository productRepo, IUserRepository userRepo)
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
            };
        }

        public async Task CreateItemChatRoom(int itemId, string buyerId, string buyerName)
        {
            string cacheKey = $"ChatRoom_{itemId}_{buyerId}";
            var semaphore = GetSemaphore(cacheKey);

            await semaphore.WaitAsync();

            try
            {
                var item = await productRepo.FindOnlyItemBy(itemId) ?? throw new ItemNotFoundException();
                if (item.SellerId == buyerId)
                {
                    throw new SelfChatCreationException();
                }
                if (await chatRepo.Has(itemId, buyerId))
                {
                    throw new ChatRoomAlreadyExistsException();
                }

                var seller = await userRepo.FindUserOnlyBy(item.SellerId) ?? throw new UserNotFoundException();
                var room = CreateRoom(itemId);
                room.JoinUser(CreateUser(buyerId, seller.UserName!));
                room.JoinUser(CreateUser(item.SellerId, buyerName));

                chatRepo.Add(room);
                item.ChatCount++;
                await chatRepo.Save();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task SendMessage()
        {

        }

        public async Task<ICollection<ChatRoomResponce>> GetUserChatRooms(string userId)
        {
            return await chatRepo.GetRoomList(userId);
        }
    }
}
