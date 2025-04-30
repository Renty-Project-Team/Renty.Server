
namespace Renty.Server.Domain.Chat
{
    public interface IChatRepository
    {
        Task CreateRoom(int itemId, string buyerId);
        bool HasRoom(int itemId, string buyerId);
    }
}
