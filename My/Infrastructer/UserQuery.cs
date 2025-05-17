using Microsoft.AspNetCore.Identity;
using Renty.Server.Auth.Domain;
using Renty.Server.My.Domain.DTO;
using Renty.Server.My.Domain.Query;

namespace Renty.Server.My.Infrastructer
{
    public class UserQuery(UserManager<Users> userManager) : IUserQuery
    {
        public async Task<ProfileResponse> GetProfile(string userId)
        {
            Users user = (await userManager.FindByIdAsync(userId))!;
            return new()
            {
                Email = user.Email!,
                Name = user.Name,
                UserName = user.UserName!,
                ProfileImage = user.ProfileImage,
                AccountNumber = user.AccountNumber,
                PhoneNumber = user.PhoneNumber!,
                MannerScore = user.MannerScore,
                TotalIncome = user.TotalIncome,
            };
        }
    }
}
