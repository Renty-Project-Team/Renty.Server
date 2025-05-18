using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Exceptions;
using Renty.Server.Transaction.Domain.DTO;
using Renty.Server.Transaction.Domain.Query;
using Renty.Server.Transaction.Service;
using System.Security.Claims;


namespace Renty.Server.Transaction.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController(TransactionService service, ITransactionQuery transactionQuery) : ControllerBase
    {
        [HttpPost("payments")]
        public async Task<IActionResult> Payments(PaymentsRequest request)
        {
            try
            {
                await service.Payments(request, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                return Ok();
            }
            catch (TradeOfferNotFoundException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "거래 요청을 찾을 수 없습니다." });
            }
            catch (TradeOfferVersionMismatchException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "거래 요청이 최신 정보와 일치하지 않습니다." });
            }
            catch (TradeOfferAlreadyAcceptedException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "이미 결제한 상품입니다." });
            }
            catch (InvalidTradeOfferDateException)
            {
                return BadRequest(new ProblemDetails() { Status = 400, Detail = "날짜가 유효하지 않습니다." });
            }
        }

        [HttpGet("seller")]
        public async Task<ActionResult<ICollection<TransactionResponse>>> GetSellerTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var transactions = await transactionQuery.FindBySeller(userId);
            return Ok(transactions);
        }

        [HttpGet("buyer")]
        public async Task<ActionResult<ICollection<TransactionResponse>>> GetBuyerTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var transactions = await transactionQuery.FindByBuyer(userId);
            return Ok(transactions);
        }
    }
}
