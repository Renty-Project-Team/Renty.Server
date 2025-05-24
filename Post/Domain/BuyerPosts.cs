using Microsoft.EntityFrameworkCore;
using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Product.Domain;

namespace Renty.Server.Post.Domain
{
    public class BuyerPosts
    {
        public int Id { get; set; } // PK

        [Comment("차용인 id")]
        public required string BuyerUserId { get; set; } // FK

        [Comment("카테고리 id")]
        public int CategoryId { get; set; } // FK

        [Comment("제목")]
        public required string Title { get; set; }

        [Comment("내용")]
        public required string Description { get; set; }

        [Comment("조회수")]
        public required int ViewCount { get; set; }

        [Comment("등록 시간")]
        public required DateTime CreatedAt { get; set; }

        [Comment("수정 시간")]
        public DateTime? UpdatedAt { get; set; }

        [Comment("삭제 시간")]
        public DateTime? DeletedAt { get; set; } // Nullable로 설정 (소프트 삭제)

        // Navigation properties
        public Users BuyerUser { get; set; }
        public required Categorys Category { get; set; }
        public ICollection<BuyerPostComments> Comments { get; set; } = [];
        public ICollection<BuyerPostImages> Images { get; set; } = [];
    }
}
