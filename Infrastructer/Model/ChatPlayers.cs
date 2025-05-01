namespace Renty.Server.Infrastructer.Model
{
    public class ChatPlayers
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int ChatRoomId { get; set; }
        public required DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }

        public Users User { get; set; }
        public ChatRooms ChatRoom { get; set; }
        public List<ChatMessages> Messages { get; set; } = [];
    }
}
