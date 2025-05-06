namespace Renty.Server.Product.Domain.DTO
{
    public class ProductDetailResponse
    {
        public required int ItemId { get; set; }
        public required string UserName { get; set; }
        public required string? UserProfileImage { get; set; }
        public required string Title { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required decimal Price { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required decimal SecurityDeposit { get; set; }
        public required int ViewCount { get; set; }
        public required int WishCount { get; set; }
        public required CategoryType[] Categories { get; set; }
        public required ItemState State { get; set; }
        public required string Description { get; set; }
        public required string[] ImagesUrl { get; set; }
    }
}
