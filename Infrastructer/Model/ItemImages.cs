namespace Renty.Server.Infrastructer.Model
{
    public class ItemImages
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public required string ImageUrl { get; set; }
        public required int Order { get; set; }
        public required DateTime CreatedAt { get; set; }

        public Items Item { get; set; }
    }
}
