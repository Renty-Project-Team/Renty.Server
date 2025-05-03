using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Renty.Server.Chat.Domain;
using Renty.Server.Chat.Domain.Repository;
using Renty.Server.Exceptions;
using Renty.Server.Global;

namespace Renty.Server.Chat.Infrastructer
{
    public class ChatRepository(RentyDbContext dbContext) : IChatRepository
    {
        
        public async Task<bool> Has(int itemId, string userId)
        {
            return await dbContext.ChatRooms
                .AnyAsync(room => room.ItemId == itemId && room.ChatUsers.Any(user => user.UserId == userId));
        }
        

        public void Add(ChatRooms room)
        {
            dbContext.ChatRooms.Add(room);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
