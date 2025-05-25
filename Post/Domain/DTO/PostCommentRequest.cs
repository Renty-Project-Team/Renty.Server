namespace Renty.Server.Post.Domain.DTO
{
    public class PostCommentRequest
    {
        public required int PostId { get; set; }
        public int? ItemId { get; set; }
        public string? Content { get; set; }
    }
}