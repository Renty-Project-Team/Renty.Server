namespace Renty.Server.Chat.Domain.DTO
{
    public class MessageResponse
    {
        public required int ChatRoomId { get; set; }
        public required string SenderId { get; set; }
        public required string Content { get; set; }
        public required MessageType Type { get; set; }
        public required DateTime Timestamp { get; set; }
    }
}
