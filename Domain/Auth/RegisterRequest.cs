namespace Renty.Server.Domain.Auth
{
    public record RegisterRequest(
        string Name, 
        string Nickname,
        string AccountNumber,
        string PhoneNumber,
        string Email,
        string Password
    );
}
