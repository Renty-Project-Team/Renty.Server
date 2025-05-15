using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Server.Auth.Domain.DTO;
using Renty.Server.Auth.Domain.Repository;
using Renty.Server.Auth.Service;
using Renty.Server.Exceptions;



namespace Renty.Server.Auth.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserRepository userRepository) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                await new AuthService(userRepository).Register(request);
                return Ok(new { Message = "회원가입 성공" });
            }
            catch (RegisterException e)
            {
                foreach (var error in e.Errors) { ModelState.AddModelError(string.Empty, error.Description); }
                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var jwt = await new AuthService(userRepository).Login(request);
                return Ok(new { Message = "로그인 성공", Token = jwt });
            }
            catch (LoginFailException)
            {
                // 실패 처리 (RequiresTwoFactor, IsLockedOut 등 확인 가능)
                return Unauthorized(new { Message = "로그인 실패: 이메일 또는 비밀번호를 확인하세요." });
            }

        }
    }
}
