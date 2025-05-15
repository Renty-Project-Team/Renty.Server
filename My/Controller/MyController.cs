using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Exceptions;
using Renty.Server.My.Domain.DTO;
using Renty.Server.My.Domain.Query;
using Renty.Server.My.Service;
using Renty.Server.Product.Domain.DTO;
using System.Security.Claims;

namespace Renty.Server.My.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MyController(MyService sevice, IWishListQuery wishListQuery) : ControllerBase
    {
        [HttpPost("wishlist")]
        public async Task<IActionResult> AddWishlist(WishListRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var itemId = request.ItemId;
                await sevice.AddWishList(itemId, userId);
                return Ok();
            }
            catch (ItemAlreadyWishListException)
            {
                return Ok();
            }
            catch (ItemNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "존재하지 않는 상품입니다." });
            }
        }

        [HttpGet("wishlist")]
        public async Task<ActionResult<ICollection<PostsResponse>>> GetWishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var wishList = await wishListQuery.GetWishList(userId);
            return Ok(wishList);
        }

        [HttpDelete("wishlist")]
        public async Task<IActionResult> RemoveWishlist(WishListRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await sevice.RemoveWishList(request.ItemId, userId);
                return Ok();
            }
            catch (ItemNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "존재하지 않는 상품입니다." });
            }
        }

        //[HttpGet("posts")]
        //public async Task<ActionResult<ICollection<PostsResponse>>> GetMyPosts()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        //    var posts = 
        //    return Ok(posts);
        //}
    }
}
