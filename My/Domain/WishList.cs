using Renty.Server.Auth.Domain;
using Renty.Server.Product.Domain;

namespace Renty.Server.My.Domain
{
    public class WishList
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required int ItemId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required Users User { get; set; }
        public required Items Item { get; set; }
    }
}
