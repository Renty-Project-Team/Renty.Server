using Renty.Server.Product.Domain;

namespace Renty.Server.Post.Domain.DTO
{
    public class BuyerPostResponse
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
        public required string Title { get; set; }
        public required CategoryType Category { get; set; }
        public required int ViewCount { get; set; }
        public required int CommentCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string ImageUrl { get; set; }
    }
}
