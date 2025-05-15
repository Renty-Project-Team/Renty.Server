namespace Renty.Server.Transaction.Domain.Repository
{
    public interface ITradeOfferRepository
    {
        Task<TradeOffers?> FindBy(int itemId, string buyerId);
        void Remove(TradeOffers offers);
        Task Save();
    }
}
