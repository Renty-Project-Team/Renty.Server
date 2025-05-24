
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Post.Domain;
using Renty.Server.Post.Domain.DTO;
using Renty.Server.Post.Domain.Repository;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.Repository;

namespace Renty.Server.Post.Service
{
    public class PostService(ICategoryRepository categoryRepo, IImageRepository imageRepo, IPostRepository postRepo)
    {
        private BuyerPosts CreatePost(PostUploadRequest request, string userId, Categorys category, ICollection<string> imgUrls)
        {
            var now = TimeHelper.GetKoreanTime();
            return new()
            {
                BuyerUserId = userId,
                Title = request.Title,
                Description = request.Description,
                ViewCount = 0,
                CreatedAt = now,
                Category = category,
                Images = [.. imgUrls.Select((url, i) => new BuyerPostImages 
                { 
                    ImageUrl = url,
                    DisplayOrder = i,
                    UploadedAt = now,
                })],
            };
        }

        public async Task<int> Upload(PostUploadRequest request, string userId)
        {
            var categorys = await categoryRepo.FindBy(request.Category) ?? throw new CategoryNotFoundException();

            try
            {
                var imgUrls = await imageRepo.SaveImages(request.Images);
                var item = CreatePost(request, userId, categorys, imgUrls);
                postRepo.Add(item);
                await postRepo.Save();
                return item.Id;
            }
            catch
            {
                imageRepo.RemoveImages();
                throw;
            }
        }
    }
}
