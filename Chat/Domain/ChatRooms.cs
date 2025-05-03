using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Product.Domain;

namespace Renty.Server.Chat.Domain
{
    public class ChatRooms
    {
        public int Id { get; set; }
        public required int ItemId { get; set; }
        public int? LastMessageId { get; set; }
        public required string RoomName { get; set; }
        public required int ChatCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Items Item { get; set; }
        public List<ChatUsers> ChatUsers { get; set; } = [];
        public ChatMessages? LastMessage { get; set; }
        public List<ChatMessages> Messages { get; set; } = [];

        public bool InUser(string userId)
        {
            return ChatUsers.Any(u => u.Equals(userId));
            {
                throw new UserNotFoundException();
            }
        }

        private bool HasOtherUsers(string senderId)
        {
            return ChatUsers.Any(u => !u.Equals(senderId));
        }

        public void JoinUser(ChatUsers user)
        {
            ChatUsers.Add(user);
        }

        //public async Task<string?> FindReceiverId(int roomId, string senderId)
        //{
        //    var room = await dbContext.ChatRooms
        //        .Include(room => room.Users)
        //        .FirstOrDefaultAsync(room => room.Id == roomId)
        //        ?? throw new HubException("{ Status: 400, Detail: 채팅방을 찾을 수 없습니다. }");

        //    if (room.Users.Any(p => p.UserId == senderId))
        //        throw new HubException("{ Status: 400, Detail: 본인이 속하지 않은 채팅방입니다. }");

        //    var receiver = room.Users.FirstOrDefault(p => p.UserId != senderId);
        //    return receiver?.UserId ?? null;
        //}

        //public ChatSendRequest SendMessage(string senderId)
        //{

        //}
    }
}
