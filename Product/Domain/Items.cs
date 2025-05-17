using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Domain;
using Renty.Server.My.Domain;
using Renty.Server.Transaction.Domain;

namespace Renty.Server.Product.Domain
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
        public required decimal SecurityDeposit { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int ViewCount { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ItemState State { get; set; } = ItemState.Active;

        public Users Seller { get; set; }
        public List<ItemImages> ItemImages { get; set; } = [];
        public List<Categorys> Categories { get; set; } = [];
        public List<ChatRooms> Chats { get; set; } = [];
        public List<TradeOffers> TradeOffers { get; set; } = [];
        public List<Transactions> Transactions { get; set; } = [];
        public List<WishList> WishLists { get; set; } = [];

        public void AddTradeOffer(TradeOffers tradeOffer)
        {
            TradeOffers.Add(tradeOffer);
        }
    }
}
