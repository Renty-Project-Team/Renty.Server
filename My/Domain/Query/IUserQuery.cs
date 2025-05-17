using Renty.Server.My.Domain.DTO;

namespace Renty.Server.My.Domain.Query
{
    public interface IUserQuery
    {
        Task<ProfileResponse> GetProfile(string userId);
    }
}
