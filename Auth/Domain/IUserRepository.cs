namespace Renty.Server.Auth.Domain
{
    public interface IUserRepository
    {
        Task AddSignToCookie(string email, string password);
        Task RemoveSignToCookie();
        Task Register(RegisterRequest request);
        Task UpdateLastLoginAt(string email);
    }
}
