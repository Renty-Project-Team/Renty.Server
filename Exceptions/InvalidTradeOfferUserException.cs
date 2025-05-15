namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class InvalidTradeOfferUserException : Exception
    {
        public InvalidTradeOfferUserException()
        {
        }

        public InvalidTradeOfferUserException(string? message) : base(message)
        {
        }

        public InvalidTradeOfferUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}