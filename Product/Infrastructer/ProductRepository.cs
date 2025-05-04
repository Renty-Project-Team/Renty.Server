using Microsoft.EntityFrameworkCore;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Repository;

namespace Renty.Server.Product.Infrastructer
{
    public class ProductRepository(RentyDbContext dbContext) : IProductRepository
    {
        public async Task<Items?> FindOnlyItemById(int itemId)
        {
            return await dbContext.Items.FirstOrDefaultAsync(i => i.Id == itemId);
        }

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

        public async Task<DetailResponse?> GetItemDetail(int itemId)
        {
            var item = await dbContext.Items
                .Include(i => i.Seller)
                .Include(i => i.ItemImages)
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null) return null;
            
            return new DetailResponse()
            {
                ItemId = item.Id,
                UserName = item.Seller.UserName!,
                UserProfileImage = item.Seller.ProfileImage,
                Title = item.Title,
                CreatedAt = item.CreatedAt,
                Price = item.Price,
                PriceUnit = item.PriceUnit,
                SecurityDeposit = item.SecurityDeposit,
                ViewCount = item.ViewCount,
                WishCount = item.WishCount,
                Categories = [.. item.Categories.Select(c => c.Name)],
                State = item.State,
                Description = item.Description,
                ImagesUrl = [.. item.ItemImages.OrderBy(img => img.Order).Select(img => img.ImageUrl)],
            };
        }

        public Task Save(Items item)
        {
            dbContext.Items.Add(item);
            return dbContext.SaveChangesAsync();
        }
    }
}
