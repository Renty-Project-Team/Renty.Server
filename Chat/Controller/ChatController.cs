using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Renty.Server.Chat.Domain;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Chat.Domain.Repository;
using Renty.Server.Chat.Service;
using Renty.Server.Exceptions;
using System.Security.Claims;

namespace Renty.Server.Chat.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController(ChatRoomService roomService) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom(ChatRoomCreateRequest request)
        {
            try
            {
                var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await roomService.CreateItemChatRoom(request.ItemId, buyerId);
                return Ok(new { Message = "채팅방이 생성되었습니다.", Status = "created" });
            }
            catch (ChatRoomAlreadyExistsException)
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
