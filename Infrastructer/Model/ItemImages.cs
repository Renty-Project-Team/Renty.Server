namespace Renty.Server.Infrastructer.Model
{
    public class ItemImages
    {
        public int Id { get; set; }
        public required int ItemId { get; set; }
        public required string ImageUrl { get; set; }
        public int Order { get; set; } = 0;
        public required DateTime CreatedAt { get; set; }

        public required Items Item { get; set; }
    }
}
