namespace Renty.Server.Chat.Domain.Query
{
    public interface IChatQuery
    {
        Task<int?> FindRoomIdBy(int itemId, string buyerId);
    }
}
