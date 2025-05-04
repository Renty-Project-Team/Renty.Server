using Renty.Server.Product.Domain.DTO;

namespace Renty.Server.Product.Domain.Repository
{
    public interface IProductRepository
    {
        Task<Items?> FindOnlyItemById(int itemId);
        Task<DetailResponse?> GetItemDetail(int itemId);
        Task Save(Items item);
        Task<ICollection<PostsResponse>> Take(PostsRequest request, int takeCount);
    }
}
