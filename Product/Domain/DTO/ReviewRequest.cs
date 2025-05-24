using Renty.Server.Attribute;
using Renty.Server.My.Domain.DTO;
using Renty.Server.Review.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Product.Domain.DTO
{
    public class ReviewRequest
    {
        public required int ItemId { get; set; } // FK to Item
        public required SellerEvaluation SellerEvaluation { get; set; } // 판매자 평가
        [Range(0, 5)]
        public required float Satisfaction { get; set; } // 예: 1.0, 2.5, 5.0
        public required string Content { get; set; }
        [MaxLength(5)]
        [AllowedExtensions(".jpeg", ".png", ".jpg", ".webp", ".gif")]
        public List<IFormFile> Images { get; set; } = [];
        public required ImageAction ImageAction { get; set; }
    }
}