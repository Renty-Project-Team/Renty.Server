using Microsoft.AspNetCore.SignalR;
using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Controller;
using Renty.Server.Chat.Domain.DTO;
using Renty.Server.Chat.Domain.Query;
using Renty.Server.Transaction.Domain;

namespace Renty.Server.Chat.Service
{
    public class ChatRoomNotificationService(IHubContext<ChatHub> chatHub, IChatQuery chatQuery)
    {
        public async Task SendTradeOfferUpdatedNotification(Users buyer, TradeOffers tradeOffer)
        {
            var roomId = await chatQuery.FindRoomIdBy(tradeOffer.ItemId, buyer.Id);
            if (roomId == null) return;

            var message = new
            {
                RoomId = roomId,
                Offer = new Offer()
                {
                    ItemId = tradeOffer.ItemId,
                    BorrowStartAt = tradeOffer.BorrowStartAt,
                    ReturnAt = tradeOffer.ReturnAt,
                    Price = tradeOffer.Price,
                    SecurityDeposit = tradeOffer.SecurityDeposit,
                    PriceUnit = tradeOffer.PriceUnit,
                    State = tradeOffer.State,
                    Title = tradeOffer.Item.Title,
                    ImageUrl = tradeOffer.Item.ItemImages.First().ImageUrl,
                    Version = tradeOffer.Version,
                },
            };
            await chatHub.Clients.User(buyer.Id).SendAsync("_handleTradeOfferUpdatedNotification", message);
        }
    }
}
