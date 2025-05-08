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
        public required int ChatCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Items Item { get; set; }
        public List<ChatUsers> ChatUsers { get; set; } = [];
        public ChatMessages? LastMessage { get; set; }
        public List<ChatMessages> Messages { get; set; } = [];

        public void AddMessage(ChatMessages message)
        {
            Messages.Add(message);
            LastMessage = message;
            UpdatedAt = message.CreatedAt;
        }

        public void JoinUser(ChatUsers user)
        {
            ChatUsers.Add(user);
        }
    }
}
