namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class TradeOfferVersionMismatchException : Exception
    {
        public TradeOfferVersionMismatchException()
        {
        }

        public TradeOfferVersionMismatchException(string? message) : base(message)
        {
        }

        public TradeOfferVersionMismatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}