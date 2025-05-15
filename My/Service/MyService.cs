using Renty.Server.Product.Domain.Repository;
using Renty.Server.Exceptions;
using Renty.Server.My.Domain;
using Renty.Server.Global;
using Renty.Server.Product.Domain;
using System.Collections;
using Renty.Server.My.Domain.DTO;
using Renty.Server.My.Domain.Query;

namespace Renty.Server.My.Service
{
    public class MyService(IProductRepository productRepo, IWishListQuery wishListQuery)
    {
        private WishList CreateWishList(int itemId, string userId)
        {
            return new()
            {
                CreatedAt = TimeHelper.GetKoreanTime(),
                ItemId = itemId,
                UserId = userId,
            };
        }

        public async Task AddWishList(int itemId, string userId)
        {
            var item = await productRepo.FindBy(itemId) ?? throw new ItemNotFoundException();
            if (item.State != ItemState.Active) throw new ItemNotFoundException(); 
            if (await wishListQuery.Has(userId, itemId)) throw new ItemAlreadyWishListException(); 

            var wishList = CreateWishList(itemId, userId);
            item.WishLists.Add(wishList);
            await productRepo.Save();
        }
    }
}
