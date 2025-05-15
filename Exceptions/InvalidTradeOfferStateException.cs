namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class InvalidTradeOfferStateException : Exception
    {
        public InvalidTradeOfferStateException()
        {
        }

        public InvalidTradeOfferStateException(string? message) : base(message)
        {
        }

        public InvalidTradeOfferStateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}