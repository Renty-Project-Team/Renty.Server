﻿namespace Renty.Server.Exceptions
{
    [Serializable]
    internal class TransactionNotFoundException : Exception
    {
        public TransactionNotFoundException()
        {
        }

        public TransactionNotFoundException(string? message) : base(message)
        {
        }

        public TransactionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}