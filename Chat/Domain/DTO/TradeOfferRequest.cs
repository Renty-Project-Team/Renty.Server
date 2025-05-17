using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Chat.Domain.DTO
{
    public class TradeOfferRequest
    {
        public required int ItemId { get; set; }
        public required string BuyerName { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "가격은 0원 보다 작을 수 없습니다.")]
        public required decimal Price { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "보증금은 0원 보다 작을 수 없습니다.")]
        public required decimal SecurityDeposit { get; set; }
        public required DateTime? BorrowStartAt { get; set; }
        public required DateTime? ReturnAt { get; set; }
    }
}