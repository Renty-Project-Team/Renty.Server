namespace Renty.Server.Infrastructer.Model
{
    public enum MessageType
    {
        Text,
        Image,
        Request,
        Delete,
    }

    public class ChatMessages
    {
        public int Id { get; set; }
        public required int ChatRoomId { get; set; }
        public required int SenderId { get; set; }
        public required string Content { get; set; }
        public required MessageType Type { get; set; }
        public required DateTime ReadAt { get; set; }
        public required DateTime CreatedAt { get; set; }

        public required ChatRooms ChatRoom { get; set; }
        public required ChatPlayers Sender { get; set; }
    }
}
