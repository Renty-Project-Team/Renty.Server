using Renty.Server.Auth.Domain;
using Renty.Server.Product.Domain;

namespace Renty.Server.Transaction.Domain
{
    public enum TradeOfferState
    {
        Pending,
        Accepted,
        Canceled,
    }

    public class TradeOffers
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public required string BuyerId { get; set; }
        public required decimal Price { get; set; }
        public required decimal SecurityDeposit { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public DateTime? BorrowStartAt { get; set; }
        public DateTime? ReturnAt { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public TradeOfferState State { get; set; } = TradeOfferState.Pending;
        public int Version { get; set; }

        public Items Item { get; set; }
        public Users Buyer { get; set; }
    }
}
