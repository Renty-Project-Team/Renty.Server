namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class ChatRoomNotFoundException : Exception
    {
        public ChatRoomNotFoundException()
        {
        }

        public ChatRoomNotFoundException(string? message) : base(message)
        {
        }

        public ChatRoomNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}