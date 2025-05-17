namespace Renty.Server.Transaction.Domain.DTO
{
    public class PaymentsRequest
    {
        public required int ItemId { get; set; }
        public required int TradeOfferVersion { get; set; }
    }
}