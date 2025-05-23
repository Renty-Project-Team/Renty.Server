﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Renty.Server.Chat.Domain;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Chat.Domain.Repository;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using System.Data;

namespace Renty.Server.Chat.Infrastructer
{
    public class ChatRepository(RentyDbContext dbContext) : IChatRepository
    {
        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task UpdateReadAt(int roomId, string userId, DateTime time)
        {
            await dbContext.ChatUsers.Where(u => u.ChatRoomId == roomId && u.UserId == userId && u.LeftAt == null)
                .ExecuteUpdateAsync(u => u.SetProperty(u => u.LastReadAt, time));
        }


        public async Task<ChatRooms?> FindBy(int roomId, DateTime lastReadAt)
        {
            return await dbContext.ChatRooms
                .Include(room => room.Item)
                .Include(room => room.LastMessage)
                .Include(room => room.Messages.Where(m => m.CreatedAt > lastReadAt))
                .Include(room => room.ChatUsers)
                    .ThenInclude(user => user.User)
                .FirstOrDefaultAsync(room => room.Id == roomId);
        }

        public async Task<ChatRooms?> FindByItem(int itemId, string userId)
        {
            return await dbContext.ChatRooms
                .Include(room => room.ChatUsers)
                .FirstOrDefaultAsync(room => room.ItemId == itemId && room.ChatUsers.Any(user => user.UserId == userId));
        }

        public async Task<ICollection<ChatRoomResponce>> GetRoomList(string userId)
        {
            return await dbContext.ChatRooms.Include(room => room.Item)
                .Include(room => room.LastMessage)
                .Include(room => room.ChatUsers)
                    .ThenInclude(user => user.User)
                .Where(room => room.ChatUsers.Any(user => user.UserId == userId && user.LeftAt == null))
                .OrderByDescending(room => room.UpdatedAt)
                .Select(room => new ChatRoomResponce
                {
                    RoomName = room.ChatUsers.First(user => user.UserId == userId).RoomName,
                    ChatRoomId = room.Id,
                    Message = room.LastMessage != null ? room.LastMessage.Content : null,
                    MessageType = room.LastMessage != null ? room.LastMessage.Type : null,
                    LastAt = room.LastMessage != null ? room.LastMessage.CreatedAt : room.CreatedAt,
                    ProfileImageUrl = room.ChatUsers.First(user => user.UserId != userId).User.ProfileImage,
                    UnreadCount = room.Messages.Where(m => m.CreatedAt > room.ChatUsers.First(user => user.UserId == userId && user.LeftAt == null).LastReadAt).Count()
                })
                .ToListAsync();
        }

        public void Add(ChatRooms room)
        {
            dbContext.ChatRooms.Add(room);
        }

        public void Remove(ChatRooms room)
        {
            dbContext.ChatRooms.Remove(room);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
