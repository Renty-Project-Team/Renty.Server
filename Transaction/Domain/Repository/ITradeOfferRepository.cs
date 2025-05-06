namespace Renty.Server.Transaction.Domain.Repository
{
    public interface ITradeOfferRepository
    {
        Task<TradeOffers?> FindBy(int itemId, string buyerId);
        Task Save();
    }
}
