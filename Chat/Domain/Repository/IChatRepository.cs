
namespace Renty.Server.Chat.Domain.Repository
{
    public interface IChatRepository
    {
        void Add(ChatRooms room);
        Task<bool> Has(int itemId, string userId);
        Task Save();
    }
}
