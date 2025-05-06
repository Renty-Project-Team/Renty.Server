using Microsoft.EntityFrameworkCore;
using Renty.Server.Transaction.Domain;
using Renty.Server.Transaction.Domain.Repository;

namespace Renty.Server.Transaction.Infrastructer
{
    public class TradeOfferRepository(RentyDbContext dbContext) : ITradeOfferRepository
    {
        public async Task<TradeOffers?> FindBy(int itemId, string buyerId)
        {
            return await dbContext.TradeOffers
                .Include(offer => offer.Item)
                    .ThenInclude(item => item.ItemImages.OrderBy(img => img.Order))
                .FirstOrDefaultAsync(offer => offer.ItemId == itemId && offer.BuyerId == buyerId);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
