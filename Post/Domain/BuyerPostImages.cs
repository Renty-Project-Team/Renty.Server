using System;

namespace Renty.Server.Post.Domain
{
    public class BuyerPostImages
    {
        public int Id { get; set; }
        public int BuyerPostId { get; set; } // FK to BuyerPosts
        public required string ImageUrl { get; set; } // URL of the image
        public required int DisplayOrder { get; set; } // Order of the image in the post
        public required DateTime UploadedAt { get; set; } // Time when the image was uploaded

        // Navigation property
        public BuyerPosts BuyerPost { get; set; } = null!; // Navigation to the associated post
    }
}
