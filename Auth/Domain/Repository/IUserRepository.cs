using Renty.Server.Auth.Domain.DTO;

namespace Renty.Server.Auth.Domain.Repository
{
    public interface IUserRepository
    {
        Task Register(RegisterRequest request);
        Task UpdateLastLoginAt(string email);
        Task<Users?> FindUserOnlyBy(string userName);
        Task<string> CreateJWT(string email, string password);
    }
}
