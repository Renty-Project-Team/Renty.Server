using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Domain.Product;
using Renty.Server.Infrastructer;
using System.Security.Claims;

namespace Renty.Server.Controllers.Product
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductUploadRepository uploadRepo, IProductFindRepository findRepo) : ControllerBase
    {
        [HttpPost("upload")]
        [Authorize]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await new UploadManager(uploadRepo, userId).Upload(request);
            return Ok(new { Message = "상품 등록을 성공했습니다." });
        }

        [HttpGet("posts")]
        public async Task<ActionResult<List<PostsResponse>>> GetPosts([FromQuery] PostsRequest request)
        {
            return await findRepo.Take(request, 20) switch
            {
                var posts when posts.Count == 0 => NotFound(new { Message = "상품이 없습니다." }),
                var posts => Ok(posts)
            };
        }
    }
}
