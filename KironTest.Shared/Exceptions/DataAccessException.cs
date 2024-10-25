﻿namespace KironTest.Shared.Exceptions
{
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
