namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class TradeOfferAlreadyAcceptedException : Exception
    {
        public TradeOfferAlreadyAcceptedException()
        {
        }

        public TradeOfferAlreadyAcceptedException(string? message) : base(message)
        {
        }

        public TradeOfferAlreadyAcceptedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}