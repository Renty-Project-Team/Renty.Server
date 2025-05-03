using Renty.Server.Model;

namespace Renty.Server.Chat.Domain
{
    public class ChatUsers
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int ChatRoomId { get; set; }
        public required DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }

        public List<ChatMessages> Messages { get; set; } = [];
        public ChatRooms ChatRoom { get; set; }
        public Users User { get; set; }

        public bool Equals(string id)
        {
            return UserId == id;
        }
    }
}
