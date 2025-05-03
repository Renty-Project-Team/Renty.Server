namespace Renty.Server.Product.Domain.Repository
{
    public interface IImageRepository
    {
        void RemoveImages();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <returns>접근 URL</returns>
        Task<ICollection<string>> SaveImages(ICollection<IFormFile> images);
    }
}
