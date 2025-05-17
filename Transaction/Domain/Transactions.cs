using Renty.Server.Auth.Domain;
using Renty.Server.Product.Domain;

namespace Renty.Server.Transaction.Domain
{
    public enum TransactionState
    {
        PaymentCompleted,
        ShippingToBuyer,
        RentalInProgress,
        RentalOverdue,
        ReturnPending,
        ReturnCompleted,
        Completed,
        CanceledBySeller,
        CanceledByBuyer,
        Disputed,
        Failed,
    }

    public class Transactions
    {
        public int Id { get; set; }
        public required int ItemId { get; set; }
        public required string BuyerId { get; set; }
        public required decimal FinalPrice { get; set; }
        public required PriceUnit PriceUnit { get; set; }
        public required decimal Price { get; set; }
        public required decimal FinalSecurityDeposit { get; set; }
        public required DateTime BorrowStartAt { get; set; }
        public required DateTime ReturnAt { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public TransactionState State { get; set; } = TransactionState.PaymentCompleted;


        public Users Buyer { get; set; }
        public Items Item { get; set; }
    }
}
