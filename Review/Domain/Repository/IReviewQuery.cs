using Renty.Server.My.Domain.DTO;

namespace Renty.Server.Review.Domain.Repository
{
    public interface IReviewQuery
    {
        Task<ICollection<ReviewResponse>> GetReviews(string userId, string userName);
        Task<ICollection<ReviewResponse>> GetReviews(int itemId, string? userName);
    }
}
