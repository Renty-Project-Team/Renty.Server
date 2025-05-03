using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace Renty.Server.Chat.Controller
{
    public class ChatHub(RentyDbContext dbContext) : Hub
    {
        [Authorize]
        public async Task SendMessage(int chatroomId, string message)
        {
            string callerId = Context.UserIdentifier!;
            
            if (string.IsNullOrEmpty(message)) throw new HubException("{ Status: 400, Detail: 메세지가 없습니다.}");
            
            
            
            
        }


    }
}
