﻿using Microsoft.AspNetCore.Identity;
using Renty.Server.Chat.Domain;
using Renty.Server.My.Domain;
using Renty.Server.Post.Domain;
using Renty.Server.Product.Domain;
using Renty.Server.Review.Domain;
using Renty.Server.Transaction.Domain;

namespace Renty.Server.Auth.Domain
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
        public string? AccountNumber { get; set; }
        public float MannerScore { get; set; }
        public string? ProfileImage { get; set; }

        public List<Items> Items { get; set; } = [];
        public List<BuyerPosts> BuyerPosts { get; set; } = [];
        public List<BuyerPostComments> BuyerPostComments { get; set; } = [];
        public List<TradeOffers> ProspectiveRentalList { get; set; } = [];
        public List<Transactions> RentalHistory { get; set; } = [];
        public List<WishList> WishLists { get; set; } = [];
        public List<ChatUsers> ChatUsers { get; set; } = [];
        public List<Reviews> Reviews { get; set; } = [];
        public List<Reviews> Reviewees { get; set; } = [];
    }
}
