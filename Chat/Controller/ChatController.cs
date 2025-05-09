using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Chat.Service;
using Renty.Server.Exceptions;
using System.Security.Claims;

namespace Renty.Server.Chat.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController(ChatService roomService) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom(ChatRoomCreateRequest request)
        {
            try
            {
                var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var buyerName = User.FindFirstValue(ClaimTypes.Name)!;
                var roomId = await roomService.CreateItemChatRoom(request.ItemId, buyerId, buyerName);
                return Ok(new { ChatRoomId = roomId, Message = "채팅방이 생성되었습니다.", Status = "created" });
            }
            catch (ChatRoomAlreadyExistsException e)
            {
                return Ok(new { ChatRoomId = e.RoomId, Message = "채팅방이 이미 존재합니다.", Status = "exists" });
            }
            catch (ItemNotFoundException)
            {
                return BadRequest(new { Message = "존재하지 않는 상품입니다." });
            }
            catch (SelfChatCreationException)
            {
                return BadRequest(new { Message = "자기 자신과 채팅방을 생성할 수 없습니다." });
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new { Message = "게시글 작성자를 찾을 수 없습니다." });
            }
        }

        [HttpGet("RoomList")]
        public async Task<ActionResult<ICollection<ChatRoomResponce>>> GetRoomList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var rooms = await roomService.GetUserChatRooms(userId);
            return Ok(new { Rooms = rooms });
        }

        [HttpGet("Room")]
        public async Task<ActionResult<ChatRoomDetailResponse>> GetRoom(int roomId, DateTime lastReadAt)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var detail = await roomService.GetRoomDetail(roomId, userId, lastReadAt);
                return Ok(detail);
            }
            catch (Exception e) when (e is ChatRoomNotFoundException or UserNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "채팅방을 찾을 수 없습니다." } );
            }
        }

        [HttpPost("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead(MaskRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await roomService.RecordReadTime(request.RoomId, userId);
                return Ok();
            }
            catch (Exception e) when (e is ChatRoomNotFoundException or UserNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "채팅방을 찾을 수 없습니다." });
            }
        }

        [HttpPost("Leave")]
        public async Task<IActionResult> LeaveRoom(LeaveRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await roomService.LeaveRoom(request.RoomId, userId);
            return Ok();
        }
    }
}
