namespace Renty.Server.Auth.Domain
{
    public class AuthManager(IUserRepository userRepository)
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
