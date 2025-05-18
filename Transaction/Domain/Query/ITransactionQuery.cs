using Renty.Server.Transaction.Domain.DTO;

namespace Renty.Server.Transaction.Domain.Query
{
    public interface ITransactionQuery
    {
        Task<ICollection<Transactions>> FindBy(string userId);
        Task<ICollection<SellerTransactionResponse>> FindBySeller(string sellerId);
    }
}
