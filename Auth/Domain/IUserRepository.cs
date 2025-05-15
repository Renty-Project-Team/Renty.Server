using Renty.Server.Model;

namespace Renty.Server.Auth.Domain
{
    public interface IUserRepository
    {
        Task Register(RegisterRequest request);
        Task UpdateLastLoginAt(string email);
        Task<Users?> FindUserOnlyBy(string userName);
        Task<string> CreateJWT(string email, string password);
    }
}
