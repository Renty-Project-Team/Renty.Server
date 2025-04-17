namespace Renty.Server.Domain.Product
{
    public interface IProductRepository
    {
        Task CreatePost(UploadRequest request, string userId, ICollection<string> imgNameList);
        void RemoveImages();
        Task<ICollection<string>> SaveImages(List<IFormFile> images);
    }
}
