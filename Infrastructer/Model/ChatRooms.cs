namespace Renty.Server.Infrastructer.Model
{
    public class ChatRooms
    {
        public int Id { get; set; }
        public required int ItemId { get; set; }
        public int? SellerId { get; set; }
        public int? BuyerId { get; set; }
        public int? LastMessageId { get; set; }
        public required int ChatCount { get; set; }
        public required int SellerUnreadCount { get; set; }
        public required int BuyerUnreadCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }


        public required Items Item { get; set; }
        public required Users Seller { get; set; }
        public required Users Buyer { get; set; }
        public ChatMessages? LastMessage { get; set; }
        public List<ChatMessages> Messages { get; set; } = [];
    }
}
