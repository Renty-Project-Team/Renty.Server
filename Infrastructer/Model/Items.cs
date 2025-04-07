namespace Renty.Server.Infrastructer.Model
{

    public enum ItemState
    {
        Active,
        Inactive,
        Deleted,
    }

    public class Items
    {
        public int Id { get; set; }
        public required string SellerId { get; set; }
        public required decimal Price { get; set; }
        public decimal SecurityDeposit { get; set; }
        public required UnitOfTime UnitOfTime { get; set; }
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int ViewCount { get; set; }
        public required int WishCount { get; set; }
        public required int ChatCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ItemState State { get; set; } = ItemState.Active;

        public required Users Seller { get; set; }
        public List<ItemImages> ItemImages { get; set; } = [];
        public List<Categorys> Categories { get; set; } = [];
        public List<ChatRooms> Chats { get; set; } = [];
        public List<TradeOffers> TradeOffers { get; set; } = [];
        public List<Transactions> Transactions { get; set; } = [];
    }
}
