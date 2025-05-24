using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.My.Domain.DTO;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.DTO;
using Renty.Server.Product.Domain.Repository;
using Renty.Server.Review.Domain;

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
                ViewCount = 0,
                CreatedAt = now,
                ItemImages = [.. imgUrls.Select((url, i) => new ItemImages()
                {
                    CreatedAt = now,
                    ImageUrl = url,
                    Order = i
                })],
            };
        }

        private Reviews CreateReview(ReviewRequest request, string reviewerId, string revieweeId)
        {
            var now = TimeHelper.GetKoreanTime();
            return new Reviews()
            {
                ItemId = request.ItemId,
                RevieweeId = revieweeId,
                ReviewerId = reviewerId,
                Satisfaction = request.Satisfaction,
                SellerEvaluation = request.SellerEvaluation,
                Content = request.Content,
                CreatedAt = now,
            };
        }

        public async Task<int> Upload(UploadRequest request, string userId)
        {
            var categorys = await categoryRepo.FindBy(request.Category) ?? throw new CategoryNotFoundException();

            try
            {
                var imgUrls = await imageRepo.SaveImages(request.Images);
                var item = CreatePost(request, userId, categorys, imgUrls);
                productRepo.Add(item);
                await productRepo.Save();
                return item.Id;
            }
            catch
            {
                imageRepo.RemoveImages();
                throw;
            }
        }
        
        public async Task<ProductDetailResponse> GetDetail(int itemId)
        {
            var item = await productRepo.FindBy(itemId) ?? throw new ItemNotFoundException();
            item.ViewCount++;
            await productRepo.Save();

            return new ProductDetailResponse()
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
                WishCount = item.WishLists.Count,
                Categories = [.. item.Categories.Select(c => c.Name)],
                State = item.State,
                Description = item.Description,
                ImagesUrl = [.. item.ItemImages.OrderBy(img => img.Order).Select(img => img.ImageUrl)],
            };
        }

        public async Task PutReview(ReviewRequest request, string reviewerId)
        {
            var item = await productRepo.FindBy(request.ItemId) ?? throw new ItemNotFoundException();
            if (item.SellerId == reviewerId) throw new SelfReviewNotAllowedException();
            if (!item.Transactions.Any(t => t.BuyerId == reviewerId)) throw new TransactionNotFoundException();
            
            var now = TimeHelper.GetKoreanTime();
            var review = item.Reviews.FirstOrDefault(r => r.ReviewerId == reviewerId);
            switch (review, request.ImageAction, request.Images)
            {
                case (null, ImageAction.Upload, not null):
                    var imgUrls = await imageRepo.SaveImages(request.Images);
                    review = CreateReview(request, reviewerId, item.SellerId);
                    review.Images = [.. imgUrls.Select((url, i) => new ReviewImages()
                    {
                        DisplayOrder = i,
                        ImageUrl = url,
                        Review = review,
                        UploadedAt = now,
                    })];
                    item.Reviews.Add(review);
                    break;
                case (null, ImageAction.None, _):
                    review = CreateReview(request, reviewerId, item.SellerId);
                    item.Reviews.Add(review);
                    break;
                case (not null, _, _):
                    review.UpdatedAt = now;
                    review.Satisfaction = request.Satisfaction;
                    review.SellerEvaluation = request.SellerEvaluation;
                    review.Content = request.Content;

                    switch (request.ImageAction, request.Images)
                    {
                        case (ImageAction.Upload, not null):
                            var newImgUrls = await imageRepo.SaveImages(request.Images);
                            review.Images = [.. newImgUrls.Select((url, i) => new ReviewImages()
                            {
                                DisplayOrder = i,
                                ImageUrl = url,
                                Review = review,
                                UploadedAt = now,
                            })];
                            break;
                        case (ImageAction.Delete, _):
                            review.Images = [];
                            break;
                    }
                    break;
            }
            
            await productRepo.Save();
        }
    }
}
