using Microsoft.AspNetCore.Identity;
using Renty.Server.Domain.Auth;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Infrastructer.Auth
{
    public class UserRepository(UserManager<Users> userManager,
        SignInManager<Users> signInManager) : IUserRepository
    {
        public async Task Register(RegisterRequest request)
        {
            var user = new Users
            {
                UserName = request.Email, // UserName은 보통 Email과 동일하게 설정
                Email = request.Email,
                Name = request.Name,
                Nickname = request.Nickname,
                AccountNumber = request.AccountNumber,
                PhoneNumber = request.PhoneNumber, // IdentityUser 속성에 저장
                CreatedAt = TimeHelper.GetKoreanTime(), // 생성 시간 기록
                State = UserState.Active, // 기본 상태
                MannerScore = 100
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new RegisterException(result.Errors);
        }

        public async Task AddSignToCookie(string email, string password)
        {
            var result = await signInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent: true, // <<-- 자동 로그인(영구 쿠키) 설정
                lockoutOnFailure: false); // 실패 시 계정 잠금 옵션

            if (!result.Succeeded) throw new LoginFailException();
        }

        public async Task UpdateLastLoginAt(string email)
        {
            // 로그인 성공! 인증 쿠키는 자동으로 응답 헤더에 포함됨.
            var user = await userManager.FindByEmailAsync(email);

            // 로그인 시간 업데이트
            user!.LastLoginAt = TimeHelper.GetKoreanTime();
            await userManager.UpdateAsync(user);
        }

        public async Task RemoveSignToCookie()
        {
            await signInManager.SignOutAsync();
        }
    }
}
