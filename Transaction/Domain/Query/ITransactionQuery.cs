namespace Renty.Server.Transaction.Domain.Query
{
    public interface ITransactionQuery
    {
        Task<ICollection<Transactions>> FindBy(string userId);

    }
}
