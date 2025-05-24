namespace Renty.Server.Post.Domain.Repository
{
    public interface IPostRepository
    {
        void Add(BuyerPosts post);
        Task Save();
    }
}
