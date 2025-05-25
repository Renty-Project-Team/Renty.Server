using Microsoft.EntityFrameworkCore;
using Renty.Server.Post.Domain;
using Renty.Server.Post.Domain.Repository;

namespace Renty.Server.Post.Infrastructer
{
    public class PostRepository(RentyDbContext dbContext) : IPostRepository
    {
        public Task<BuyerPosts?> FindBy(int postId)
        {
            return dbContext.BuyerPosts
                .Include(p => p.BuyerUser)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public void Add(BuyerPosts post)
        {
            dbContext.BuyerPosts.Add(post);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
