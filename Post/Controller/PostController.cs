using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Post.Domain.DTO;
using Renty.Server.Post.Service;
using System.Security.Claims;

namespace Renty.Server.Post.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(PostService service) : ControllerBase
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
        public async Task<IActionResult> GetPosts()
        {
            // Handle fetching posts logic here
            // For example, retrieve posts from a database
            return Ok(new { Message = "Posts retrieved successfully." });
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetPost(int postId)
        {
            // Handle fetching a single post by ID logic here
            // For example, retrieve the post from a database
            return Ok(new { Message = $"Post {postId} retrieved successfully." });
        }
    }
}
