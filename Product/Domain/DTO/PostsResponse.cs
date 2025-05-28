using Renty.Server.Product.Domain;

namespace Renty.Server.Product.Domain.DTO
{
    public class PostsResponse
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
        public required string Title { get; set; }
        public required decimal Price { get; set; }
        public required decimal Deposit { get; set; }
        public required List<CategoryType> Categorys { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required int ViewCount { get; set; }
        public required int WishCount { get; set; }
        public required int ChatCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string ImageUrl { get; set; }
        public required ItemState State { get; set; }
    }
}
