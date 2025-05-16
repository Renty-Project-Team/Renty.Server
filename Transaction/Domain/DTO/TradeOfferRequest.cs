using Renty.Server.Product.Domain;
using Renty.Server.Transaction.Domain;

namespace Renty.Server.Transaction.Domain.DTO
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