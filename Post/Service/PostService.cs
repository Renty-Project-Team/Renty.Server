
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Post.Domain;
using Renty.Server.Post.Domain.DTO;
using Renty.Server.Post.Domain.Repository;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.Query;
using Renty.Server.Product.Domain.Repository;

namespace Renty.Server.Post.Service
{
    public class PostService(ICategoryRepository categoryRepo, IImageRepository imageRepo, IPostRepository postRepo, IProductQuery productQuery)
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

        private BuyerPostComments CreateComment(PostCommentRequest request, string userId)
        {
            var now = TimeHelper.GetKoreanTime();
            return new()
            {
                UserId = userId,
                ItemId = request.ItemId,
                Content = request.Content,
                UpdatedAt = now,
                CreatedAt = now,
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

        public async Task Comment(PostCommentRequest request, string userId)
        {
            var post = await postRepo.FindBy(request.PostId) ?? throw new PostNotFoundException();

            if (request.ItemId is not null && await productQuery.Has(request.ItemId.Value, userId) is false)
            {
                throw new ItemNotFoundException();
            }

            var commnet = CreateComment(request, userId);

            post.Comments.Add(commnet);
            await postRepo.Save();
        }

        public async Task Delete(int postId, string userId)
        {
            var post = await postRepo.FindBy(postId) ?? throw new PostNotFoundException();
            if (post.BuyerUserId != userId) throw new NotPostOwnerException();

            post.DeletedAt = TimeHelper.GetKoreanTime();
            
            await postRepo.Save();
        }
    }
}
