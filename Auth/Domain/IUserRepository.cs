using Renty.Server.Model;

namespace Renty.Server.Auth.Domain
{
    public interface IUserRepository
    {
        Task AddSignToCookie(string email, string password);
        Task RemoveSignToCookie();
        Task Register(RegisterRequest request);
        Task UpdateLastLoginAt(string email);
        Task<Users?> FindUserOnlyBy(string id);
    }
}
