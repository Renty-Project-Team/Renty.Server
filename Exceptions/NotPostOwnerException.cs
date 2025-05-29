namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class NotPostOwnerException : Exception
    {
        public NotPostOwnerException()
        {
        }

        public NotPostOwnerException(string? message) : base(message)
        {
        }

        public NotPostOwnerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}