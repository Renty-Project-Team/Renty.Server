namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class TradeOfferNotFoundException : Exception
    {
        public TradeOfferNotFoundException()
        {
        }

        public TradeOfferNotFoundException(string? message) : base(message)
        {
        }

        public TradeOfferNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}