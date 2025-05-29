using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Exceptions;
using Renty.Server.My.Domain.DTO;
using Renty.Server.My.Domain.Query;
using Renty.Server.My.Service;
using Renty.Server.Post.Domain.DTO;
using Renty.Server.Post.Domain.Repository;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Query;
using Renty.Server.Review.Domain.Repository;
using System.Security.Claims;

namespace Renty.Server.My.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MyController(MyService service, IWishListQuery wishListQuery, IUserQuery userQuery, IProductQuery productQuery, 
        IReviewQuery reviewQuery, IPostQuery postQuery) : ControllerBase
    {
        [HttpPost("wishlist")]
        public async Task<IActionResult> AddWishlist(WishListRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var itemId = request.ItemId;
                await service.AddWishList(itemId, userId);
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
                await service.RemoveWishList(request.ItemId, userId);
                return Ok();
            }
            catch (ItemNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "존재하지 않는 상품입니다." });
            }
        }

        [HttpGet("posts")]
        public async Task<ActionResult<ICollection<PostsResponse>>> GetMyPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var posts = await productQuery.GetMyPosts(userId);
            return Ok(posts);
        }

        [HttpGet("buyer-posts")]
        public async Task<ActionResult<ICollection<BuyerPostResponse>>> GetMyBuyerPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var posts = await postQuery.GetMyBuyerPosts(userId);
            return Ok(posts);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ProfileResponse>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var profile = await userQuery.GetProfile(userId);
            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileRequest request)
        {
            try
            {
                var userName = User.FindFirstValue(ClaimTypes.Name)!;
                var jwt = await service.UpdateProfile(userName, request);
                return Ok(new { Token = jwt });
            }
            catch (UpdateProfileException e)
            {
                foreach (var error in e.Errors) { ModelState.AddModelError(string.Empty, error.Description); }
                return BadRequest(ModelState);
            }
        }

        [HttpGet("review")]
        public async Task<ActionResult<ICollection<ReviewResponse>>> GetReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var userName = User.FindFirstValue(ClaimTypes.Name)!;
            return Ok(await reviewQuery.GetReviews(userId, userName));
        }
    }
}
