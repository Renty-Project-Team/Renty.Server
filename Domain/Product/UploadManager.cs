using Microsoft.EntityFrameworkCore;

namespace Renty.Server.Domain.Product
{
    public class UploadManager(IProductUploadRepository product, string userId)
    {
        public async Task Upload(UploadRequest request)
        {
            try
            {
                var names = await product.SaveImages(request.Images);
                await product.CreatePost(request, userId, names);
            }
            catch (DbUpdateException)
            {
                product.RemoveImages();
                throw;
            }
        }
    }
}
