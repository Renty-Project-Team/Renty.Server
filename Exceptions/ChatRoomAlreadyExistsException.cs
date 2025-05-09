namespace Renty.Server.Exceptions
{
    public class ChatRoomAlreadyExistsException : Exception
    {
        public required int RoomId { get; init; }
    }
}
