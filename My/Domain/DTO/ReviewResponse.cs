using Renty.Server.Review.Domain;

namespace Renty.Server.My.Domain.DTO
{
    public class ReviewResponse
    {
        public required int ItemId { get; set; }
        public required string ItemTitle { get; set; } 
        public required string ItemImageUrl { get; set; } 
        public required string? MyName { get; set; }
        public required string SellerName { get; set; }
        public required string? SellerProfileImageUrl { get; set; }
        public required string BuyerName { get; set; }
        public required string? BuyerProfileImageUrl { get; set; }
        public required double Satisfaction { get; set; }
        public required string Content { get; set; }
        public required SellerEvaluation SellerEvaluation { get; set; }
        public required ICollection<string> ImagesUrl { get; set; }
        public required DateTime WritedAt { get; set; }
    }
}