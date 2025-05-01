
using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Domain.Chat
{
    public interface IChatCreateRepository
    {
        Task ValidationSellerNotSameBuyerAndHasRoom(int itemId, string userId);
        Task Save();
        void JoinUser(ChatRooms room, string userId);
        ChatRooms CreateRoom(int itemId);
    }
}
