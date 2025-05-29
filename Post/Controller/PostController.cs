using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Exceptions;
using Renty.Server.Post.Domain.DTO;
using Renty.Server.Post.Domain.Repository;
using Renty.Server.Post.Service;
using System.Security.Claims;

namespace Renty.Server.Post.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(PostService service, IPostQuery postQuery) : ControllerBase
    {
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] PostUploadRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                var postId = await service.Upload(request, userId);
                return Ok(new { PostId = postId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "File upload failed.", Error = ex.Message });
            }

        }

        [HttpGet("posts")]
        public async Task<ActionResult<ICollection<BuyerPostResponse>>> GetPosts([FromQuery] BuyerPostsRequest request)
        {
            return Ok(await postQuery.Take(request));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<BuyerPostDetailResponse>> GetPost(int postId)
        {
            try
            {
                return Ok(await postQuery.Detail(postId));
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "게시글을 찾을 수 없습니다.", });
            }
        }

        [Authorize]
        [HttpPost("comment")]
        public async Task<IActionResult> Comment(PostCommentRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                if (request.ItemId is null && string.IsNullOrWhiteSpace(request.Content))
                {
                    return BadRequest(new ProblemDetails() { Status = 400, Detail = "상품을 첨부하거나 글을 작성해주세요." });
                }

                await service.Comment(request, userId);
                return Ok();
            }
            catch (PostNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "존재하지 않는 게시글입니다." });
            }
            catch (ItemNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "해당 상품이 존재하지 않거나, 본인의 상품이 아닙니다." });
            }
        }

        [Authorize]
        [HttpDelete("post")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                await service.Delete(postId, userId);
                return Ok();
            }
            catch (PostNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "존재하지 않는 게시글입니다." });
            }
            catch (NotPostOwnerException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "해당 게시글의 작성자가 아닙니다." });
            }
        }
    }
}
