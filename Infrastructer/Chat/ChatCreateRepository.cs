using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Renty.Server.Domain.Chat;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Infrastructer.Chat
{
    public class ChatCreateRepository(RentyDbContext dbContext) : IChatCreateRepository
    {
        public ChatRooms CreateRoom(int itemId)
        {
            var now = TimeHelper.GetKoreanTime();

            ChatRooms chatRoom = new()
            {
                ItemId = itemId,
                ChatCount = 0,
                CreatedAt = now,
                UpdatedAt = now,
            };

            dbContext.ChatRooms.Add(chatRoom);
            return chatRoom;
            //await dbContext.SaveChangesAsync();
        }

        public async Task ValidationSellerNotSameBuyerAndHasRoom(int itemId, string userId)
        {
            var item = await dbContext.Items
                .Include(item => item.Chats.Where(room => room.Players.Any(p => p.UserId == userId)))
                .FirstOrDefaultAsync(item => item.Id == itemId)
                ?? throw new ItemNotFoundException();
            
            if (item.SellerId == userId) throw new SelfChatCreationException();
            if (item.Chats.Count > 0) throw new HasChatRoomException();
        }

        public void JoinUser(ChatRooms room, string userId)
        {
            var now = TimeHelper.GetKoreanTime();
            var player = new ChatPlayers()
            {
                ChatRoom = room,
                UserId = userId,
                JoinedAt = now,
            };
            
            dbContext.ChatPlayers.Add(player);
        }

        public async Task<string> FindReceiverId(int chatroomId, string senderId)
        {
            //var room = await dbContext.ChatRooms.FirstOrDefaultAsync(room => room.Id == chatroomId);
            //if (room == null) 
            //    throw new HubException("{ Status: 400, Detail: 채팅방을 찾을 수 없습니다. }");
            //if (room.BuyerId != senderId && room.SellerId != senderId)
            //    throw new HubException("{ Status: 400, Detail: 본인이 속하지 않은 채팅방입니다. }");

            //string receiverId = room.BuyerId == senderId ? room.SellerId : room.BuyerId;

            //if (room.)

            //return receiverId;
            return null;
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
