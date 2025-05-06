using Renty.Server.Product.Domain.DTO;

namespace Renty.Server.Product.Domain.Repository
{
    public interface IProductRepository
    {
        void Add(Items item);
        Task<Items?> FindBy(int itemId);
        Task Save();
        Task<ICollection<PostsResponse>> Take(PostsRequest request, int takeCount);
    }
}
