using Microsoft.EntityFrameworkCore;
using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Product.Domain;

namespace Renty.Server.Post.Domain
{
    public class BuyerPostComments
    {
        public int Id { get; set; } // PK

        [Comment("유저 id")]
        public required string UserId { get; set; } // FK

        [Comment("제안 아이템 id")]
        public int? ItemId { get; set; } // FK

        [Comment("댓글 포스트 id")]
        public required int BuyerPostId { get; set; } // FK

        [Comment("내용")]
        public string? Content { get; set; }

        [Comment("등록 시간")]
        public required DateTime CreatedAt { get; set; }

        [Comment("수정 시간")]
        public required DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Users User { get; set; }
        public Items? Item { get; set; }
        public BuyerPosts BuyerPost { get; set; }
    }
}
