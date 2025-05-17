using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Transaction.Domain;
using Renty.Server.Transaction.Domain.DTO;
using Renty.Server.Transaction.Domain.Repository;
using System;

namespace Renty.Server.Transaction.Service
{
    public class TransactionService(ITradeOfferRepository tradeOfferRepo)
    {
        private Transactions CreateTransaction(TradeOffers tradeOffer, string buyerId)
        {
            return new Transactions()
            {
                ItemId = tradeOffer.ItemId,
                BuyerId = buyerId,
                PriceUnit = tradeOffer.PriceUnit,
                Price = tradeOffer.Price,
                FinalPrice = tradeOffer.CalculateFinalPrice(),
                FinalSecurityDeposit = tradeOffer.SecurityDeposit,
                CreatedAt = TimeHelper.GetKoreanTime(),
                BorrowStartAt = tradeOffer.BorrowStartAt!.Value,
                ReturnAt = tradeOffer.ReturnAt!.Value,
            };
        }

        public async Task Payments(PaymentsRequest request, string buyerId)
        {
            var tradeOffer = await tradeOfferRepo.FindBy(request.ItemId, buyerId) ?? throw new TradeOfferNotFoundException();
            tradeOffer.PaymentsValidate(request.TradeOfferVersion);

            tradeOffer.State = TradeOfferState.Accepted;
            var transaction = CreateTransaction(tradeOffer, buyerId);
            tradeOffer.Item.Transactions.Add(transaction);
            await tradeOfferRepo.Save();
        }
    }
}
