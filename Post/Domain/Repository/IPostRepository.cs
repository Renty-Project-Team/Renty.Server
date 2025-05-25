namespace Renty.Server.Post.Domain.Repository
{
    public interface IPostRepository
    {
        void Add(BuyerPosts post);
        Task<BuyerPosts?> FindBy(int postId);
        Task Save();
    }
}
