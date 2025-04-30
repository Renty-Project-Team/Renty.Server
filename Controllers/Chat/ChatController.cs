using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Domain.Chat;
using Renty.Server.Exceptions;
using Renty.Server.Infrastructer.Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Renty.Server.Controllers.Chat
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController(IChatRepository chatRepo) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom(ChatRoomCreateRequest request)
        {
            try
            {
                var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await new ChatManager().CreateRoom(request.ItemId, buyerId, chatRepo);
                return Ok(new { Message = "채팅방이 생성되었습니다.", Status = "created" });
            }
            catch (HasChatRoomException)
            {
                return Ok(new { Message = "채팅방이 이미 존재합니다.", Status = "exists" });
            }
            catch (ItemNotFoundException)
            {
                return BadRequest(new { Message = "존재하지 않는 상품입니다." });
            }
            catch (SelfChatCreationException)
            {
                return BadRequest(new { Message = "자기 자신과 채팅방을 생성할 수 없습니다." });
            }
        }
    }
}
