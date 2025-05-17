using Renty.Server.Auth.Domain;
using Renty.Server.Exceptions;
using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Transaction.Domain
{
    public enum TradeOfferState
    {
        Pending,
        Accepted,
        Canceled,
    }

    public class TradeOffers
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public required string BuyerId { get; set; }
        public required decimal Price { get; set; }
        public required decimal SecurityDeposit { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public DateTime? BorrowStartAt { get; set; }
        public DateTime? ReturnAt { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public TradeOfferState State { get; set; } = TradeOfferState.Pending;
        public int Version { get; set; }

        public Items Item { get; set; }
        public Users Buyer { get; set; }

        public void PaymentsValidate(int version)
        {
            if (Version != version) throw new TradeOfferVersionMismatchException();
            if (State == TradeOfferState.Accepted) throw new TradeOfferAlreadyAcceptedException();

            if (BorrowStartAt is null || ReturnAt is null || BorrowStartAt > ReturnAt) 
                throw new InvalidTradeOfferDateException();
        }

        public decimal CalculateFinalPrice()
        {
            DateTime sDate = BorrowStartAt!.Value.Date;
            DateTime eDate = ReturnAt!.Value.Date;

            var numberOfUnits = PriceUnit switch
            {
                PriceUnit.Day => (int)(eDate - sDate).TotalDays + 1,
                PriceUnit.Week => (int)Math.Ceiling((eDate - sDate).TotalDays + 1 / 7.0) ,
                PriceUnit.Month => ((eDate.Year - sDate.Year) * 12) + eDate.Month - sDate.Month + 1,
            };

            var totalPrice = numberOfUnits * Price;
            return totalPrice;
        }
    }
}
