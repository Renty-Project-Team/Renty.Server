using Renty.Server.Product.Domain;

namespace Renty.Server.Post.Domain.DTO
{
    public class BuyerPostDetailResponse
    {
        public required int PostId { get; set; }
        public required string UserName { get; set; }
        public required string? UserProfileImage { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required CategoryType Category { get; set; }
        public required ICollection<string> ImagesUrl { get; set; }
        public required int ViewCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required ICollection<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public required int CommentId { get; set; }
        public required string UserName { get; set; }
        public string? Content { get; set; }
        public required DateTime CreatedAt { get; set; }
        public ItemDetail? ItemDetail { get; set; }
    }

    public class ItemDetail
    {
        public required int ItemId { get; set; } // FK to Items
        public required string Title { get; set; }
        public required decimal Price { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required decimal Deposit { get; set; }
        public required string ImageUrl { get; set; }
    }
}
