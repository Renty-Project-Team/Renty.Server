namespace Renty.Server.My.Domain.Repository
{
    public interface IWishListRepository
    {
        Task Remove(string userId, int itemId);
    }
}
