namespace Renty.Server.Infrastructer.Model
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
        public required int ItemId { get; set; }
        public required string BuyerId { get; set; }
        public required decimal Price { get; set; }
        public required decimal FinalPrice { get; set; }
        public required decimal SecurityDeposit { get; set; }
        public required UnitOfTime UnitOfTime { get; set; }
        public DateTime? BorrowStartAt { get; set; }
        public DateTime? ReturnAt { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public TradeOfferState State { get; set; } = TradeOfferState.Pending;


        public required Items Item { get; set; }
        public required Users Buyer { get; set; }
    }
}
