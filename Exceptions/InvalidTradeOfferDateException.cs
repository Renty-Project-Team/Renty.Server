namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class InvalidTradeOfferDateException : Exception
    {
        public InvalidTradeOfferDateException()
        {
        }

        public InvalidTradeOfferDateException(string? message) : base(message)
        {
        }

        public InvalidTradeOfferDateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}