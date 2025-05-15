using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Renty.Server.Auth.Domain;
using Renty.Server.Exceptions;
using Renty.Server.Global;
using Renty.Server.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Renty.Server.Auth.Infrastructer
{
    public class UserRepository(UserManager<Users> userManager, IConfiguration configuration) : IUserRepository
    {
        private string GenerateJwtToken(Users user) // Users는 Identity 사용자 모델
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 사용자 클레임 설정
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, user.Id.ToString()), // 사용자의 고유 ID (PK)
                new(ClaimTypes.Name, user.UserName!),                 // 사용자 이름 (로그인 ID)
                new(ClaimTypes.Email, user.Email!),                   // 사용자 이메일

                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (고유 식별자)
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID (고유성 보장)
                new("username", user.UserName!), // 커스텀 클레임
            ];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<Users?> FindUserOnlyBy(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        public async Task Register(RegisterRequest request)
        {
            var user = new Users
            {
                UserName = request.UserName, // UserName은 보통 Email과 동일하게 설정
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber, // IdentityUser 속성에 저장
                CreatedAt = TimeHelper.GetKoreanTime(), // 생성 시간 기록
                State = UserState.Active, // 기본 상태
                MannerScore = 100
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new RegisterException(result.Errors);
        }

        public async Task<string> CreateJWT(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email) ?? throw new LoginFailException();
            if (!await userManager.CheckPasswordAsync(user, password)) throw new LoginFailException();

            return GenerateJwtToken(user); // JWT 토큰 생성
        }

        public async Task UpdateLastLoginAt(string email)
        {
            // 로그인 성공! 인증 쿠키는 자동으로 응답 헤더에 포함됨.
            var user = await userManager.FindByEmailAsync(email);

            // 로그인 시간 업데이트
            user!.LastLoginAt = TimeHelper.GetKoreanTime();
            await userManager.UpdateAsync(user);
        }
    }
}
