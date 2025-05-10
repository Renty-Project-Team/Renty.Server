using Microsoft.Extensions.Options;
using Renty.Server.Global;
using Renty.Server.Product.Domain.Repository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


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

                var image = await Image.LoadAsync(img.OpenReadStream());
                image.Mutate(x => x.Resize(550, 0));
                await image.SaveAsync(savePath, new JpegEncoder()
                {
                    Quality = 80,
                });

                return Path.Combine(imageUrlBase, fileName);
            }));
        }

        public void RemoveImages()
        {
            if (Directory.Exists(imageFolder)) Directory.Delete(imageFolder, true);
        }
    }
}
