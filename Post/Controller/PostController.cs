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
    }
}
