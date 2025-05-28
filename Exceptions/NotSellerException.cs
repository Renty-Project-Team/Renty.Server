namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class NotSellerException : Exception
    {
        public NotSellerException()
        {
        }

        public NotSellerException(string? message) : base(message)
        {
        }

        public NotSellerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}