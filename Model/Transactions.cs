using Renty.Server.Product.Domain;

namespace Renty.Server.Model
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
        public required decimal FinalSecurityDeposit { get; set; }
        public required DateTime BorrowStartAt { get; set; }
        public required DateTime ReturnAt { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public TransactionState State { get; set; } = TransactionState.PaymentCompleted;


        public required Users Buyer { get; set; }
        public required Items Item { get; set; }
    }
}
