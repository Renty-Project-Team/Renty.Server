namespace Renty.Server.Auth.Domain
{
    public class AuthManager(IUserRepository userRepository)
    {
        public async Task Register(RegisterRequest request)
        {
            await userRepository.Register(request);
        }

        public async Task Login(LoginRequest request)
        {
            await userRepository.AddSignToCookie(request.Email, request.Password);
            await userRepository.UpdateLastLoginAt(request.Email);
        }

        public async Task Logout()
        {
            await userRepository.RemoveSignToCookie();
        }
    }
}
