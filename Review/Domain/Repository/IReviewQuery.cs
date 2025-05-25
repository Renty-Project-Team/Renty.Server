using Renty.Server.My.Domain.DTO;

namespace Renty.Server.Review.Domain.Repository
{
    public interface IReviewQuery
    {
        Task<ICollection<ReviewResponse>> GetReviews(string userId, string userName);
    }
}
