using Microsoft.EntityFrameworkCore;
using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Review.Domain
{
    public enum SellerEvaluation
    {
        None = 0,
        Good = 1,
        Bad = 2
    }

    public class Reviews
    {
        public int Id { get; set; } // PK

        [Comment("판매자 id")]
        public required string RevieweeId { get; set; } // FK to User

        [Comment("구매자 id")]
        public required string ReviewerId { get; set; } // FK to User

        [Comment("아이템 id")]
        public required int ItemId { get; set; } // FK to Item

        [Comment("만족도 (5점 만점)")]
        public required float Satisfaction { get; set; } // 예: 1.0, 2.5, 5.0

        [EnumDataType(typeof(SellerEvaluation))]
        public required SellerEvaluation SellerEvaluation { get; set; }

        [Comment("리뷰 내용")]
        public required string Content { get; set; }

        [Comment("리뷰 작성 시간")]
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Users Reviewee { get; set; } // 판매자 (리뷰 대상)
        public Users Reviewer { get; set; }  // 구매자 (리뷰 작성자)
        public Items Item { get; set; }
        public ICollection<ReviewImages> Images { get; set; } = [];
    }
}
