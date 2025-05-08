using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Renty.Server.Chat.Domain;
using Renty.Server.Chat.Service;
using Renty.Server.Exceptions;
using System.Security.Claims;


namespace Renty.Server.Chat.Controller
{
    [Authorize]
    public class ChatHub(ChatService chatService) : Hub
    {
        public async Task SendMessage(int roomId, string message, MessageType type)
        {
            try
            {
                string callerId = Context.UserIdentifier!;
                string userName = Context.User?.FindFirstValue(ClaimTypes.Name)!;

                if (string.IsNullOrEmpty(message)) throw new HubException("{ Status: 400, Detail: 메세지가 없습니다. }");
                var response = await chatService.SendMessage(roomId, callerId, userName, message, type);
                
                await Clients.User(response.receiverId).SendAsync("ReceiveMessage", response.message);
            }
            catch (Exception e) when (e is ChatRoomNotFoundException or UserNotFoundException)
            {
                throw new HubException("{ Status: 400, Detail: 채팅방을 찾을 수 없습니다. }");
            }
        }

        public async Task SendMessageToUser(string userName, string message)
        {
            string callerId = Context.UserIdentifier!;
            await Clients.User(callerId).SendAsync("ReceiveMessage", userName, message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
