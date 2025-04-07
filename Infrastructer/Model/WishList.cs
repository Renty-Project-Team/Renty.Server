namespace Renty.Server.Infrastructer.Model
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
