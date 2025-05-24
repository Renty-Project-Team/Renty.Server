using Renty.Server.Product.Domain;

namespace Renty.Server.Transaction.Domain.DTO
{
    public class TransactionResponse
    {
        public required int ItemId { get; set; }
        public required int? RoomId { get; set; }
        public required string ItemImageUrl { get; set; }
        public required string Title { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required decimal Price { get; set; }
        public required decimal FinalPrice { get; set; }
        public required decimal FinalSecurityDeposit { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime BorrowStartAt { get; set; }
        public required DateTime ReturnAt { get; set; }
        public required string Name { get; set; }
        public required string? ProfileImage { get; set; }
        public required TransactionState State { get; set; }
    }
}
