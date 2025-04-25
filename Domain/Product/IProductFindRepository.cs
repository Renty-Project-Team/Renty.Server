using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Domain.Product
{
    public interface IProductFindRepository
    {
        Task<ICollection<PostsResponse>> Take(PostsRequest request, int takeCount);
    }
}
