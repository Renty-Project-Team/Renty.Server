namespace Renty.Server.Infrastructer.Model
{
    public class Sessions
    {
        public required Guid Id { get; set; }
        public required int UserId { get; set; }
        public required DateTime ExpiredAt { get; set; }

        public required Users User { get; set; }
    }
}
