using Renty.Server.Auth.Domain.DTO;
using Renty.Server.Auth.Domain.Repository;

namespace Renty.Server.Auth.Service
{
    public class AuthService(IUserRepository userRepository)
    {
        public async Task Register(RegisterRequest request)
        {
            await userRepository.Register(request);
        }

        public async Task<string> Login(LoginRequest request)
        {
            var jwt = await userRepository.CreateJWT(request.Email, request.Password);
            await userRepository.UpdateLastLoginAt(request.Email);
            return jwt;
        }
    }
}
