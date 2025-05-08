namespace Renty.Server.Chat.Domain
{

    public class ChatMessages
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public required string Content { get; set; }
        public required MessageType Type { get; set; }
        public required DateTime CreatedAt { get; set; }

        public ChatRooms ChatRoom { get; set; }
        public ChatUsers Sender { get; set; }
    }
}
