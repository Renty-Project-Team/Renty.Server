namespace Renty.Server.My.Domain.DTO
{
    public class ProfileResponse
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string UserName { get; set; }
        public required string? ProfileImage { get; set; }
        public required string? AccountNumber { get; set; }
        public required string PhoneNumber { get; set; }
        public required float MannerScore { get; set; }
        public required decimal TotalIncome { get; set; }
    }
}
