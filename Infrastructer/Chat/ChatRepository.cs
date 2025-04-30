using Renty.Server.Domain.Chat;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Infrastructer.Chat
{
    public class ChatRepository(RentyDbContext dbContext) : IChatRepository
    {
        public async Task CreateRoom(int itemId, string buyerId)
        {
            var item = dbContext.Items.FirstOrDefault(item => item.Id == itemId)
                ?? throw new ItemNotFoundException();

            var sellerId = item.SellerId;

            if (buyerId == sellerId) throw new SelfChatCreationException();

            var now = TimeHelper.GetKoreanTime();

            ChatRooms chatRoom = new()
            {
                ItemId = itemId,
                SellerId = sellerId,
                BuyerId = buyerId,
                ChatCount = 0,
                SellerUnreadCount = 0,
                BuyerUnreadCount = 0,
                CreatedAt = now,
                UpdatedAt = now,
            };

            dbContext.ChatRooms.Add(chatRoom);
            await dbContext.SaveChangesAsync();
        }

        public bool HasRoom(int itemId, string buyerId)
        {
            return dbContext.ChatRooms
                .Where(room => room.ItemId == itemId && room.BuyerId == buyerId).Any();
        }
    }
}
