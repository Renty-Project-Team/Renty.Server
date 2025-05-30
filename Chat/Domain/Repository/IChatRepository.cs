﻿
using Microsoft.EntityFrameworkCore.Storage;
using Renty.Server.Chat.Domain.DTO;

namespace Renty.Server.Chat.Domain.Repository
{
    public interface IChatRepository
    {
        void Add(ChatRooms room);
        Task<IDbContextTransaction> BeginTransaction();
        Task Commit(IDbContextTransaction transaction);
        Task<ChatRooms?> FindBy(int roomId, DateTime lastReadAt);
        Task<ChatRooms?> FindByItem(int itemId, string userId);
        Task<ICollection<ChatRoomResponce>> GetRoomList(string userId);
        void Remove(ChatRooms room);
        Task Save();
        Task UpdateReadAt(int roonId, string userId, DateTime time);
    }
}
