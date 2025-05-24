using Microsoft.EntityFrameworkCore;

namespace Renty.Server.Review.Domain
{
    public class ReviewImages
    {
        public int Id { get; set; } // PK

        [Comment("리뷰 id")]
        public int ReviewId { get; set; } // FK to Review

        [Comment("이미지 경로")]
        public required string ImageUrl { get; set; }

        [Comment("이미지 순서")]
        public required int DisplayOrder { get; set; }

        [Comment("업로드 시간")]
        public required DateTime UploadedAt { get; set; }

        // Navigation property
        public required Reviews Review { get; set; }
    }
}
