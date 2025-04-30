using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Domain.Auth
{
    public record RegisterRequest
    (
        [StringLength(20, ErrorMessage = "최대 20글자 까지 가능합니다.")]
        string Name,
        [StringLength(8, MinimumLength = 2, ErrorMessage ="최소 2글자, 최대 8글자 까지 가능합니다.")]
        [RegularExpression(@"^[^!@#$%^&*()+=\[{\]};:\""'<>,./?~\\ ]+$", 
            ErrorMessage = "사용자 이름에 특수문자를 포함할 수 없습니다.")]
        string UserName,
        string PhoneNumber,
        [EmailAddress]
        string Email,
        [StringLength(30, ErrorMessage = "최대 30자리 까지 설정할 수 있습니다.")]
        string Password
    );
}
