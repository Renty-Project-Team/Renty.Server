using Microsoft.Extensions.Options;
using Renty.Server.Domain.Product;
using Renty.Server.Global;
using Renty.Server.Infrastructer.Model;
using System.Linq;
using System.Xml.Linq;


namespace Renty.Server.Infrastructer.Product
{
    public class ProductUploadRepository : IProductUploadRepository
    {
        private string imageFolder;
        private string imageUrlBase;
        private RentyDbContext dbContext;

        public ProductUploadRepository(RentyDbContext dbContext, IOptions<Settings> options)
        {
            string imageStorage = Path.Combine(options.Value.DataStorage, options.Value.ImagesFolder);
            string folder = Guid.NewGuid().ToString();
            this.dbContext = dbContext;
            imageFolder = Path.Combine(imageStorage, folder);
            imageUrlBase = Path.Combine(options.Value.ImagesUrl, folder);
        }

        public async Task<ICollection<string>> SaveImages(List<IFormFile> images)
        {
            if (!Directory.Exists(imageFolder)) Directory.CreateDirectory(imageFolder);

            return await Task.WhenAll(images.Select(async img =>
            {
                var extension = Path.GetExtension(img.FileName);
                var fileName = Guid.NewGuid().ToString() + extension;
                var savePath = Path.Combine(imageFolder, fileName);

                using var fileStream = new FileStream(savePath, FileMode.Create);
                await img.CopyToAsync(fileStream);

                return fileName;
            }));
        }

        public async Task CreatePost(UploadRequest request, string userId, ICollection<string> imgNameList)
        {
            var now = TimeHelper.GetKoreanTime();
            Items item = new()
            {
                SellerId = userId,
                Price = request.Price,
                PriceUnit = request.Unit,
                SecurityDeposit = request.Deposit,
                Title = request.Title,
                Description = request.Description,
                Categories = [.. dbContext.Categorys.Where(c => c.Name == request.Category)],
                ChatCount = 0,
                ViewCount = 0,
                WishCount = 0,
                CreatedAt = now,
                ItemImages = [.. imgNameList.Select((name, i) => new ItemImages()
                {
                    CreatedAt = now,
                    ImageUrl = Path.Combine(imageUrlBase, name),
                    Order = i
                })],
            };

            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync();
        }

        public void RemoveImages()
        {
            if (Directory.Exists(imageFolder)) Directory.Delete(imageFolder, true);
        }
    }
}
