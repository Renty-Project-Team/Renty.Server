using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Chat.Domain.DTO
{
    public class MaskRequest
    {
        [Range(1, int.MaxValue)]
        public int RoomId { get; set; }
    }
}
