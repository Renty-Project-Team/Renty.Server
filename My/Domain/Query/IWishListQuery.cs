using Renty.Server.My.Domain.DTO;
using Renty.Server.Product.Domain.DTO;

namespace Renty.Server.My.Domain.Query
{
    public interface IWishListQuery
    {
        Task<ICollection<PostsResponse>> GetWishList(string userId);
        Task<bool> Has(string userId, int itemId);
    }
}
