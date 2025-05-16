using Microsoft.EntityFrameworkCore;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Query;

namespace Renty.Server.Product.Infrastructer
{
    public class ProductQuery(RentyDbContext dbContext) : IProductQuery
    {
        public async Task<ICollection<PostsResponse>> GetMyPosts(string userId)
        {
            return await dbContext.Items.Where(item => item.SellerId == userId)
                .OrderByDescending(item => item.CreatedAt)
                .Select(item => new PostsResponse()
                {
                    Id = item.Id,
                    UserName = item.Seller.UserName!,
                    Title = item.Title,
                    Price = item.Price,
                    PriceUnit = item.PriceUnit,
                    Deposit = item.SecurityDeposit,
                    Categorys = item.Categories.Select(c => c.Name).ToList(),
                    WishCount = item.WishLists.Count,
                    ViewCount = item.ViewCount,
                    ChatCount = item.Chats.Count,
                    CreatedAt = item.CreatedAt,
                    ImageUrl = item.ItemImages.OrderBy(img => img.Order).First().ImageUrl,
                })
                .ToListAsync();
        }
    }
}
