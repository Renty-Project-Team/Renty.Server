using Microsoft.EntityFrameworkCore;
using Renty.Server.Transaction.Domain;
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
    }
}
