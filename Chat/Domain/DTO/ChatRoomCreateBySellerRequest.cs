namespace Renty.Server.Chat.Domain.DTO
{
    public class ChatRoomCreateBySellerRequest
    {
        public int ItemId { get; set; }
        public required string BuyerName { get; set; }
    }
}