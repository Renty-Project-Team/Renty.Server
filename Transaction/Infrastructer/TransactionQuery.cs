using Microsoft.EntityFrameworkCore;
using Renty.Server.Transaction.Domain;
using Renty.Server.Transaction.Domain.DTO;
using Renty.Server.Transaction.Domain.Query;

namespace Renty.Server.Transaction.Infrastructer
{
    public class TransactionQuery(RentyDbContext dbContext) : ITransactionQuery
    {
        public async Task<decimal> GetTotalIncome(string userId)
        {
            return await dbContext.Transactions
                .Where(t => t.Item.SellerId == userId)
                .SumAsync(t => t.FinalPrice);
        }

        public async Task<ICollection<TransactionResponse>> FindBySeller(string sellerId)
        {
            return await dbContext.Transactions
                .Include(t => t.Item)
                    .ThenInclude(i => i.ItemImages)
                .Include(t => t.Buyer)
                .Include(t => t.Item)
                    .ThenInclude(i => i.Chats)
                        .ThenInclude(c => c.ChatUsers)
                .Where(t => t.Item.SellerId == sellerId)
                .Select(t =>
                    new TransactionResponse()
                    {
                        ItemId = t.ItemId,
                        RoomId = t.Item.Chats.FirstOrDefault(c => c.ChatUsers.All(u => (u.UserId == sellerId && u.LeftAt == null) || u.UserId == t.BuyerId))!.Id,
                        Title = t.Item.Title,
                        PriceUnit = t.PriceUnit,
                        Price = t.Price,
                        FinalPrice = t.FinalPrice,
                        FinalSecurityDeposit = t.FinalSecurityDeposit,
                        CreatedAt = t.CreatedAt,
                        BorrowStartAt = t.BorrowStartAt,
                        ReturnAt = t.ReturnAt,
                        Name = t.Buyer.UserName!,
                        State = t.State,
                        ItemImageUrl = t.Item.ItemImages.FirstOrDefault()!.ImageUrl,
                    }
                )
                .ToListAsync();
        }

        public async Task<ICollection<TransactionResponse>> FindByBuyer(string buyerId)
        {
            return await dbContext.Transactions
                .Include(t => t.Item)
                    .ThenInclude(i => i.Seller)
                .Include(t => t.Item)
                    .ThenInclude(i => i.ItemImages)
                .Include(t => t.Item)
                    .ThenInclude(i => i.Chats)
                        .ThenInclude(c => c.ChatUsers)
                .Include(t => t.Buyer)
                .Where(t => t.BuyerId == buyerId)
                .Select(t =>
                    new TransactionResponse()
                    {
                        ItemId = t.ItemId,
                        RoomId = t.Item.Chats.FirstOrDefault(c => c.ChatUsers.All(u => (u.UserId == buyerId && u.LeftAt == null) || u.UserId == t.Item.SellerId))!.Id,
                        Title = t.Item.Title,
                        PriceUnit = t.PriceUnit,
                        Price = t.Price,
                        FinalPrice = t.FinalPrice,
                        FinalSecurityDeposit = t.FinalSecurityDeposit,
                        CreatedAt = t.CreatedAt,
                        BorrowStartAt = t.BorrowStartAt,
                        ReturnAt = t.ReturnAt,
                        Name = t.Item.Seller.UserName!,
                        State = t.State,
                        ItemImageUrl = t.Item.ItemImages.FirstOrDefault()!.ImageUrl,
                    }
                )
                .ToListAsync();
        }
    }
}
