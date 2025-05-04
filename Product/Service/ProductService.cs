using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Repository;

namespace Renty.Server.Product.Service
{
    public class ProductService(IProductRepository productRepo, IImageRepository imageRepo, ICategoryRepository categoryRepo)
    {
        private Items CreatePost(UploadRequest request, string userId, Categorys category, ICollection<string> imgUrls)
        {
            var now = TimeHelper.GetKoreanTime();
            return new()
            {
                SellerId = userId,
                Price = request.Price,
                PriceUnit = request.Unit,
                SecurityDeposit = request.Deposit,
                Title = request.Title,
                Description = request.Description,
                Categories = [category],
                ChatCount = 0,
                ViewCount = 0,
                WishCount = 0,
                CreatedAt = now,
                ItemImages = [.. imgUrls.Select((url, i) => new ItemImages()
                {
                    CreatedAt = now,
                    ImageUrl = url,
                    Order = i
                })],
            };
        }

        public async Task Upload(UploadRequest request, string userId)
        {
            var categorys = await categoryRepo.FindBy(request.Category) ?? throw new CategoryNotFoundException();

            try
            {
                var imgUrls = await imageRepo.SaveImages(request.Images);
                var item = CreatePost(request, userId, categorys, imgUrls);
                productRepo.Add(item);
                await productRepo.Save();
            }
            catch
            {
                imageRepo.RemoveImages();
                throw;
            }
        }
        
        public async Task<DetailResponse> GetDetail(int itemId)
        {
            var item = await productRepo.FindBy(itemId) ?? throw new ItemNotFoundException();
            item.ViewCount++;
            await productRepo.Save();

            return new DetailResponse()
            {
                ItemId = item.Id,
                UserName = item.Seller.UserName!,
                UserProfileImage = item.Seller.ProfileImage,
                Title = item.Title,
                CreatedAt = item.CreatedAt,
                Price = item.Price,
                PriceUnit = item.PriceUnit,
                SecurityDeposit = item.SecurityDeposit,
                ViewCount = item.ViewCount,
                WishCount = item.WishCount,
                Categories = [.. item.Categories.Select(c => c.Name)],
                State = item.State,
                Description = item.Description,
                ImagesUrl = [.. item.ItemImages.OrderBy(img => img.Order).Select(img => img.ImageUrl)],
            };
        }
    }
}
