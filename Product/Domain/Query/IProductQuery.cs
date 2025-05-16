using Renty.Server.Product.Domain.DTO;

namespace Renty.Server.Product.Domain.Query
{
    public interface IProductQuery
    {
        Task<ICollection<PostsResponse>> GetMyPosts(string userId);
    }
}
