using Renty.Server.Post.Domain.DTO;

namespace Renty.Server.Post.Domain.Repository
{
    public interface IPostQuery
    {
        Task<BuyerPostDetailResponse> Detail(int postId);

        Task<ICollection<BuyerPostResponse>> Take(BuyerPostsRequest request);
    }
}
