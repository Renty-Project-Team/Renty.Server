using Microsoft.EntityFrameworkCore;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Repository;

namespace Renty.Server.Product.Infrastructer
{
    public class ProductRepository(RentyDbContext dbContext) : IProductRepository
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
                    WishCount = i.WishLists.Count,
                    ViewCount = i.ViewCount,
                    ChatCount = i.Chats.Count,
                    CreatedAt = i.CreatedAt,
                    ImageUrl = i.ItemImages.OrderBy(img => img.Order).First().ImageUrl,
                }).ToListAsync();
        }

        public async Task<Items?> FindBy(int itemId)
        {
            return await dbContext.Items
                .Include(i => i.Seller)
                .Include(i => i.ItemImages)
                .Include(i => i.Categories)
                .Include(i => i.Transactions)
                .FirstOrDefaultAsync(i => i.Id == itemId);
        }

        public void Add(Items item)
        {
            dbContext.Items.Add(item);
        }

        public Task Save()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
