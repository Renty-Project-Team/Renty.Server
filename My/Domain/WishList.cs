using Renty.Server.Auth.Domain;
using Renty.Server.Product.Domain;

namespace Renty.Server.My.Domain
{
    public class WishList
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required int ItemId { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Users User { get; set; }
        public Items Item { get; set; }
    }
}
