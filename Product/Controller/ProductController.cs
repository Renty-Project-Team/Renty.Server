using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Repository;
using Renty.Server.Product.Service;
using System.Security.Claims;

namespace Renty.Server.Product.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(UploadService uploadService, IProductRepository product) : ControllerBase
    {
        [HttpPost("upload")]
        [Authorize]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await uploadService.Upload(request, userId);
            return Ok(new { Message = "상품 등록을 성공했습니다." });
        }

        [HttpGet("posts")]
        public async Task<ActionResult<List<PostsResponse>>> GetPosts([FromQuery] PostsRequest request)
        {
            return await product.Take(request, 20) switch
            {
                var posts when posts.Count == 0 => NotFound(new ProblemDetails()
                {
                    Status = 404,
                    Detail = "조건을 만족하는 게시글이 없습니다."
                }),
                var posts => Ok(posts)
            };
        }
    }
}
