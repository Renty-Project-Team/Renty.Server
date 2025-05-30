﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("Create_by_seller")]
        public async Task<IActionResult> CreateRoom(ChatRoomCreateBySellerRequest request)
        {
            try
            {
                var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var sellerName = User.FindFirstValue(ClaimTypes.Name)!;
                if (sellerName == request.BuyerName) throw new SelfChatCreationException();

                var roomId = await roomService.CreateBuyerChatRoom(request.ItemId, sellerId, request.BuyerName);
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
                return BadRequest(new { Message = "거래가 완료된 사람만 채팅방을 만들 수 있습니다." });
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await roomService.RecordReadTime(request.RoomId, userId);
            return Ok();
        }

        [HttpPost("Leave")]
        public async Task<IActionResult> LeaveRoom(LeaveRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await roomService.LeaveRoom(request.RoomId, userId);
                return Ok();
            }
            catch (Exception e) when (e is ChatRoomNotFoundException or UserNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "채팅방을 찾을 수 없습니다." });
            }
        }

        [HttpPost("TradeOffer")]
        public async Task<IActionResult> UpdateTradeOffer(TradeOfferRequest request)
        {
            try
            {
                var callerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await roomService.UpdateTradeOffer(request, callerId);
                return Ok();
            }
            catch (Exception e) when (e is UserNotFoundException or TradeOfferNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "거래 요청을 찾을 수 없습니다." });
            }
            catch (InvalidTradeOfferUserException) // 요청자의 거래가 아닐경우
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "잘못된 거래 요청입니다." });
            }
            catch (InvalidTradeOfferStateException) // 거래가 취소되었거나 이미 완료된 경우
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "거래 요청을 수정할 수 없습니다." });
            }
            catch (InvalidTradeOfferDateException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "날짜가 잘못되었습니다." });
            }
        }       
    }
}
