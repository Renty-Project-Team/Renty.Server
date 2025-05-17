using Microsoft.AspNetCore.Identity;

namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class UpdateProfileException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }

        public UpdateProfileException()
        {
        }

        public UpdateProfileException(IEnumerable<IdentityError> errors)
        {
            Errors = errors;
        }

        public UpdateProfileException(string? message) : base(message)
        {
        }

        public UpdateProfileException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}