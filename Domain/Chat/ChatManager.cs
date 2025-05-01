using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Renty.Server.Exceptions;

namespace Renty.Server.Domain.Chat
{
    public class ChatManager
    {
        private static TimeSpan slidingExpiration = TimeSpan.FromMinutes(30);
        private readonly IMemoryCache memoryCache;

        public ChatManager(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        private SemaphoreSlim GetSemaphore(string cacheKey)
        {
            var lazySemaphore = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.SlidingExpiration = slidingExpiration;
                entry.RegisterPostEvictionCallback((cacheKey, value, reason, state) =>
                {
                    // 캐시에서 제거될 때 SemaphoreSlim Dispose
                    if (value is Lazy<SemaphoreSlim> lazy && lazy.IsValueCreated)
                    {
                        lazy.Value.Dispose(); // Dispose 호출
                    }
                });

                return new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1));
            });

            return lazySemaphore!.Value;
        }

        public async Task CreateItemChatRoom(int itemId, string buyerId, IChatCreateRepository chatRepo)
        {
            string cacheKey = $"ChatRoom_{itemId}_{buyerId}";
            var semaphore = GetSemaphore(cacheKey);

            await semaphore.WaitAsync();

            try
            {
                await chatRepo.ValidationSellerNotSameBuyerAndHasRoom(itemId, buyerId);
                var room = chatRepo.CreateRoom(itemId);
                chatRepo.JoinUser(room, buyerId);
                await chatRepo.Save();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<string> FindOtherUserId(string userId)
        {
            return null;
        }
    }
}
