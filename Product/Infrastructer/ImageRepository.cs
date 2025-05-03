using Microsoft.Extensions.Options;
using Renty.Server.Global;
using Renty.Server.Product.Domain.Repository;
using System.Xml.Linq;

namespace Renty.Server.Product.Infrastructer
{
    public class ImageRepository : IImageRepository
    {
        private readonly string imageFolder;
        private readonly string imageUrlBase;

        public ImageRepository(IOptions<Settings> options)
        {
            string folder = Guid.NewGuid().ToString();
            string imageStorage = Path.Combine(options.Value.DataStorage, options.Value.ImagesFolder);
            imageFolder = Path.Combine(imageStorage, folder);
            imageUrlBase = Path.Combine(options.Value.ImagesUrl, folder);
        }

        public async Task<ICollection<string>> SaveImages(ICollection<IFormFile> images)
        {
            if (!Directory.Exists(imageFolder)) Directory.CreateDirectory(imageFolder);

            return await Task.WhenAll(images.Select(async img =>
            {
                var extension = Path.GetExtension(img.FileName);
                var fileName = Guid.NewGuid().ToString() + extension;
                var savePath = Path.Combine(imageFolder, fileName);

                using var fileStream = new FileStream(savePath, FileMode.Create);
                await img.CopyToAsync(fileStream);

                return Path.Combine(imageUrlBase, fileName);
            }));
        }

        public void RemoveImages()
        {
            if (Directory.Exists(imageFolder)) Directory.Delete(imageFolder, true);
        }
    }
}
