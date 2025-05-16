using Microsoft.AspNetCore.Identity;
using Renty.Server.Auth.Domain;
using Renty.Server.Auth.Domain.Query;

namespace Renty.Server.Auth.Infrastructer
{
    public class UserQuery(UserManager<Users> userManager) : IUserQuery
    {
        public async Task<string?> GetProfileImageUrl(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return user!.ProfileImage;
        }
    }
}
