using Renty.Server.Product.Domain;

namespace Renty.Server.Chat.Domain.DTO
{
    public class TradeOfferRequest
    {
        public required int ItemId { get; set; }
        public required string BuyerName { get; set; }
        public required decimal Price { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required decimal SecurityDeposit { get; set; }
        public required DateTime? BorrowStartAt { get; set; }
        public required DateTime? ReturnAt { get; set; }
    }
}