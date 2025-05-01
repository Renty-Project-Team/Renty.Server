namespace Renty.Server.Infrastructer.Model
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
        public List<ChatPlayers> Players { get; set; } = [];
        public ChatMessages? LastMessage { get; set; }
        public List<ChatMessages> Messages { get; set; } = [];
    }
}
