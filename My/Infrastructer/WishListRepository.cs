using Microsoft.EntityFrameworkCore;
using Renty.Server.My.Domain.Repository;

namespace Renty.Server.My.Infrastructer
{
    public class WishListRepository(RentyDbContext dbContext) : IWishListRepository
    {
        public async Task Remove(string userId, int itemId)
        {
            await dbContext.WishLists.Where(w => w.UserId == userId && w.ItemId == itemId)
                .ExecuteDeleteAsync();
        }
    }
}
