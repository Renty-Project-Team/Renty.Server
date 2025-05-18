using Microsoft.EntityFrameworkCore;
using Renty.Server.Transaction.Domain;
using Renty.Server.Transaction.Domain.DTO;
using Renty.Server.Transaction.Domain.Query;

namespace Renty.Server.Transaction.Infrastructer
{
    public class TransactionQuery(RentyDbContext dbContext) : ITransactionQuery
    {
        public async Task<ICollection<Transactions>> FindBy(string userId)
        {
            return await dbContext.Transactions
                .Where(t => t.BuyerId == userId)
                .ToListAsync();
        }

        public async Task<ICollection<SellerTransactionResponse>> FindBySeller(string sellerId)
        {
            return await dbContext.Transactions
                .Include(t => t.Item)
                .Include(t => t.Buyer)
                .Where(t => t.Item.SellerId == sellerId)
                .Select(t =>
                    new SellerTransactionResponse()
                    {
                        ItemId = t.ItemId,
                        Title = t.Item.Title,
                        PriceUnit = t.PriceUnit,
                        Price = t.Price,
                        FinalPrice = t.FinalPrice,
                        FinalSecurityDeposit = t.FinalSecurityDeposit,
                        CreatedAt = t.CreatedAt,
                        BorrowStartAt = t.BorrowStartAt,
                        ReturnAt = t.ReturnAt,
                        BuyerName = t.Buyer.Name,
                        State = t.State,
                    }
                )
                .ToListAsync();
        }
    }
}
