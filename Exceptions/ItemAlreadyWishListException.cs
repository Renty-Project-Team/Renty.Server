namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class ItemAlreadyWishListException : Exception
    {
        public ItemAlreadyWishListException()
        {
        }

        public ItemAlreadyWishListException(string? message) : base(message)
        {
        }

        public ItemAlreadyWishListException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}