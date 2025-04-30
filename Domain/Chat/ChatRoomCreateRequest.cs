using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Domain.Chat
{
    public class ChatRoomCreateRequest
    {
        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }
    }
}
