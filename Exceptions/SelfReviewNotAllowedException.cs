namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class SelfReviewNotAllowedException : Exception
    {
        public SelfReviewNotAllowedException()
        {
        }

        public SelfReviewNotAllowedException(string? message) : base(message)
        {
        }

        public SelfReviewNotAllowedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}