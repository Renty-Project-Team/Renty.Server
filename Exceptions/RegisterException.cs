using Microsoft.AspNetCore.Identity;

namespace Renty.Server.Exceptions
{
    public class RegisterException(IEnumerable<IdentityError> errors) : Exception
    {
        public IEnumerable<IdentityError> Errors { get; set; } = errors;
    }
}
