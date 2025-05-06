namespace Renty.Server.Chat.Domain.DTO
{
    public class ChatRoomResponce
    {
        public required int ChatRoomId { get; set; }
        public required string RoomName { get; set; }
        public required string? ProfileImageUrl { get; set; }
        public required MessageType? MessageType { get; set; }
        public required string? Message { get; set; }
        public required DateTime? LastAt { get; set; }
        public required int UnreadCount { get; set; }
    }
}
