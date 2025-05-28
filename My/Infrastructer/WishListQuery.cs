using Microsoft.EntityFrameworkCore;
using Renty.Server.My.Domain.DTO;
using Renty.Server.My.Domain.Query;
using Renty.Server.Product.Domain.DTO;

namespace Renty.Server.My.Infrastructer
{
    public class WishListQuery(RentyDbContext dbContext) : IWishListQuery
    {
        public async Task<ICollection<PostsResponse>> GetWishList(string userId)
        {
            return await dbContext.WishLists
                .Include(w => w.Item)
                    .ThenInclude(i => i.Seller)
                .Where(w => w.UserId == userId)
                .Select(w => new PostsResponse()
                {
                    Id = w.Item.Id,
                    UserName = w.Item.Seller.UserName!,
                    Title = w.Item.Title,
                    Price = w.Item.Price,
                    PriceUnit = w.Item.PriceUnit,
                    Deposit = w.Item.SecurityDeposit,
                    Categorys = w.Item.Categories.Select(c => c.Name).ToList(),
                    WishCount = w.Item.WishLists.Count,
                    ViewCount = w.Item.ViewCount,
                    ChatCount = w.Item.Chats.Count,
                    CreatedAt = w.Item.CreatedAt,
                    ImageUrl = w.Item.ItemImages.OrderBy(img => img.Order).First().ImageUrl,
                    State = w.Item.State
                })
                .ToListAsync();
        }

        public async Task<bool> Has(string userId, int itemId)
        {
            return await dbContext.WishLists
                .AnyAsync(w => w.UserId == userId && w.ItemId == itemId);
        }
    }
}
