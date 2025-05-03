
using Renty.Server.Chat.Domain.DTO;

namespace Renty.Server.Chat.Domain.Repository
{
    public interface IChatRepository
    {
        void Add(ChatRooms room);
        Task<ICollection<ChatRoomResponce>> GetRoomList(string userId);
        Task<bool> Has(int itemId, string userId);
        Task Save();
    }
}
