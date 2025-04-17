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
    public class ProductController(IProductRepository product) : ControllerBase
    {
        [HttpPost("upload")]
        [Authorize]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await new UploadManager(product, userId).Upload(request);
            return Ok(new { Message = "상품 등록을 성공했습니다." });
        }

    }
}
