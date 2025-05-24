using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Post.Domain.DTO
{
    public class BuyerPostsRequest
    {
        public CategoryType? Category { get; set; }
        public DateTime? MaxCreatedAt { get; set; }
        [MaxLength(3)] 
        public List<string> TitleWords { get; set; } = [];
    }
}