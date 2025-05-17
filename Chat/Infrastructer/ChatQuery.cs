using Microsoft.EntityFrameworkCore;
using Renty.Server.Chat.Domain.Query;

namespace Renty.Server.Chat.Infrastructer
{
    public class ChatQuery(RentyDbContext dbContext) : IChatQuery
    {
        public async Task<int?> FindRoomIdBy(int itemId, string buyerId)
        {
            return (await dbContext.ChatRooms.FirstOrDefaultAsync(room =>
                    room.ItemId == itemId &&
                    room.ChatUsers.Any(user => user.UserId == buyerId && user.LeftAt == null))
                )
                ?.Id;
        }
    }
}
