using Microsoft.EntityFrameworkCore;
using Renty.Server.My.Domain.DTO;
using Renty.Server.Review.Domain.Repository;

namespace Renty.Server.Review.Infrastructer
{
    public class ReviewQuery(RentyDbContext dbContext) : IReviewQuery
    {
        public async Task<ICollection<ReviewResponse>> GetReviews(string userId, string userName)
        {
            return await dbContext.Reviews
                .Where(r => r.RevieweeId == userId || r.ReviewerId == userId)
                .Select(r => new ReviewResponse
                {
                    ItemId = r.ItemId,
                    ItemTitle = r.Item.Title,
                    ItemImageUrl = r.Item.ItemImages.First().ImageUrl ?? string.Empty,
                    MyName = userName,
                    SellerName = r.Reviewee.UserName!,
                    SellerProfileImageUrl = r.Reviewee.ProfileImage,
                    BuyerName = r.Reviewer.UserName!,
                    BuyerProfileImageUrl = r.Reviewer.ProfileImage,
                    Satisfaction = r.Satisfaction,
                    Content = r.Content,
                    SellerEvaluation = r.SellerEvaluation,
                    ImagesUrl = r.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList(),
                    WritedAt = r.CreatedAt
                })
                .ToListAsync();
        }

    }
}
