using Microsoft.AspNetCore.SignalR;
using Renty.Server.Exceptions;

namespace Renty.Server.Domain.Chat
{
    public class ChatManager()
    {
        public async Task CreateRoom(int itemId, string buyerId, IChatRepository chatRepo)
        {
            if (chatRepo.HasRoom(itemId, buyerId)) throw new HasChatRoomException();
            await chatRepo.CreateRoom(itemId, buyerId);
        }
    }
}
