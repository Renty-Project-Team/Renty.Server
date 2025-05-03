using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Chat.Domain.DTO
{
    public class ChatRoomCreateRequest
    {
        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }
    }
}
