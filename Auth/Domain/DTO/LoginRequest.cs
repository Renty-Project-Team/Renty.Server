using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Auth.Domain.DTO
{
    public record LoginRequest
    (
        [EmailAddress]
        string Email,
        [StringLength(30, ErrorMessage = "최대 30자리 까지 설정할 수 있습니다.")]
        string Password
    );
}
