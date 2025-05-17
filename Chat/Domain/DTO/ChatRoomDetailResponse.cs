using Renty.Server.Product.Domain;
using Renty.Server.Transaction.Domain;

namespace Renty.Server.Chat.Domain.DTO
{
    public class ChatRoomDetailResponse
    {
        public int RoomId { get; set; }
        
        public required Offer Offer { get; set; }
        public required string CallerName { get; set; }
        public required bool IsSeller { get; set; }
        public required List<User> Users { get; set; }
        public required List<Message> Messages { get; set; }
    }

    public class Offer
    {
        public required int ItemId { get; set; }
        public required string Title { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required decimal SecurityDeposit { get; set; }
        public required DateTime? BorrowStartAt { get; set; }
        public required DateTime? ReturnAt { get; set; }
        public required TradeOfferState State { get; set; }
        public required int Version { get; set; }
    }

    public class User
    {
        public required string Name { get; set; }
        public required string? ProfileImageUrl { get; set; }
        public required DateTime LastReadAt { get; set; }
    }

    public class Message
    {
        public required string SenderName { get; set; }
        public required DateTime SendAt { get; set; }
        public required string Content { get; set; }
        public required MessageType Type { get; set; }
    }
}
