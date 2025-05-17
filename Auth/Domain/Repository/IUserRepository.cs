using Microsoft.AspNetCore.Identity;
using Renty.Server.Auth.Domain.DTO;

namespace Renty.Server.Auth.Domain.Repository
{
    public interface IUserRepository
    {
        Task Register(RegisterRequest request);
        Task UpdateLastLoginAt(string email);
        Task<Users?> FindBy(string userName);
        Task<string> Login(string email, string password);
        Task<IdentityResult> Update(Users user);
        string GenerateJwtToken(Users user);
    }
}
