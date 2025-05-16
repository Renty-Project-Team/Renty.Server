namespace Renty.Server.Auth.Domain.Query
{
    public interface IUserQuery
    {
        Task<string?> GetProfileImageUrl(string userId);
    }
}
