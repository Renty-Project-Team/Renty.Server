﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Exceptions;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Repository;
using Renty.Server.Product.Service;
using System.Security.Claims;

namespace Renty.Server.Product.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ProductService productService, IProductRepository product) : ControllerBase
    {
        [HttpPost("upload")]
        [Authorize]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var itemId = await productService.Upload(request, userId);
            return Ok(new { ItemId = itemId, Message = "상품 등록을 성공했습니다." });
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

        [HttpGet("detail")]
        public async Task<ActionResult<ProductDetailResponse>> GetPost([FromQuery] int itemId)
        {
            try
            {
                var post = await productService.GetDetail(itemId);
                return Ok(post);
            }
            catch (ItemNotFoundException)
            {
                return NotFound(new ProblemDetails() { Status = 404, Detail = "존재하지 않는 게시글입니다." });
            }
        }

        [HttpPut("review")]
        [Authorize]
        public async Task<IActionResult> Review([FromForm] ReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                await productService.PutReview(request, userId);
                return Ok(new { Message = "리뷰 등록을 성공했습니다." });
            }
            catch (ItemNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "상품을 찾을 수 없습니다." });
            }
            catch (TransactionNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "거래 내역이 없습니다." });
            }
            catch (SelfReviewNotAllowedException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "자신에게 리뷰를 작성할 수 없습니다." });
            }
        }
    }
}
