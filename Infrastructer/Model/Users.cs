using Microsoft.AspNetCore.Identity;

namespace Renty.Server.Infrastructer.Model
{
    public enum Provider
    {
        Google,
        Kakao,
        Naver,
    }

    public enum UserState
    {
        Active,
        Inactive,
        Deleted,
    }

    public class Users : IdentityUser
    {
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Provider? Provider { get; set; }
        public UserState State { get; set; } = UserState.Active;
        public required string Name { get; set; }
        public required string Nickname { get; set; }
        public string? AccountNumber { get; set; }
        public float MannerScore { get; set; }
        public string? ProfileImage { get; set; }
        public int TotalIncome { get; set; }

        public List<Items> Items { get; set; } = [];
        public List<ChatRooms> SellerChats { get; set; } = [];
        public List<ChatRooms> BuyerChats { get; set; } = [];
        public List<TradeOffers> ProspectiveRentalList { get; set; } = [];
        public List<Transactions> RentalHistory { get; set; } = [];
        public List<WishList> WishLists { get; set; } = [];
    }
}
