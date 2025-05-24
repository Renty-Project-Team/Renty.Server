using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Renty.Server.Post.Domain.DTO;
using Renty.Server.Post.Domain.Repository;
using Renty.Server.Product.Domain.DTO;
using System.Reflection.Metadata.Ecma335;

namespace Renty.Server.Post.Infrastructer
{
    public class PostQuery(RentyDbContext dbContext) : IPostQuery
    {
        public async Task<ICollection<BuyerPostResponse>> Take(BuyerPostsRequest request)
        {
            var query = dbContext.BuyerPosts.AsNoTracking().AsQueryable();

            if (request.Category != null)
            {
                query = query.Where(post => post.Category.Name == request.Category);
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
                .Take(20)
                .Select(post =>
                    new BuyerPostResponse()
                    {
                        Category = post.Category.Name,
                        CommentCount = post.Comments.Count,
                        CreatedAt = post.CreatedAt,
                        Id = post.Id,
                        ImageUrl = post.Images.Count > 0
                            ? post.Images.OrderBy(img => img.DisplayOrder).First().ImageUrl
                            : string.Empty,
                        Title = post.Title,
                        UserName = post.BuyerUser.UserName!,
                        ViewCount = post.ViewCount
                    }
                )  
                .ToListAsync();
        }
    }
}
