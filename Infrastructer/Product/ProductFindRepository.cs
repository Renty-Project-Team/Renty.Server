using Microsoft.EntityFrameworkCore;
using Renty.Server.Domain.Product;
using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Infrastructer.Product
{
    public class ProductFindRepository(RentyDbContext dbContext) : IProductFindRepository
    {
        public async Task<ICollection<PostsResponse>> Take(PostsRequest request, int takeCount)
        {
            var query = dbContext.Items.AsQueryable();

            if (request.Categorys.Count != 0)
            {
                query = query.Where(item => item.Categories.Any(c => request.Categorys.Contains(c.Name)));
            }

            if (request.MaxCreatedAt != null)
            {
                query = query.Where(i => i.CreatedAt < request.MaxCreatedAt);
            }

            if (request.TitleWords.Count != 0)
            {
                var words = request.TitleWords.Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => w.Trim()).Distinct().ToArray();
                if (words.Length > 0)
                {
                    query = query.Where(i => 
                        words.Any(w => i.Title.Contains(w))
                    );
                }
            }

            return await query
                .OrderByDescending(i => i.CreatedAt)
                .Take(takeCount)
                .Select(i => new PostsResponse()
                {
                    Id = i.Id,
                    UserName = i.Seller.UserName!,
                    Title = i.Title,
                    Price = i.Price,
                    PriceUnit = i.PriceUnit,
                    Deposit = i.SecurityDeposit,
                    Categorys = i.Categories.Select(c => c.Name).ToList(),
                    WishCount = i.WishCount,
                    ViewCount = i.ViewCount,
                    ChatCount = i.ChatCount,
                    CreatedAt = i.CreatedAt,
                    ImageUrl = i.ItemImages.OrderBy(img => img.Order).First().ImageUrl,
                }).ToListAsync();
        }
    }
}
