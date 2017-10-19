using System;
using System.Runtime.Serialization;

namespace TimeConverter.Exceptions
{
    [Serializable]
    public class InvalidDateTimeFormatException : TimeConverterException
    {
        public InvalidDateTimeFormatException()
        {
        }

        public InvalidDateTimeFormatException(string message) 
            : base(message)
        {
        }

        public InvalidDateTimeFormatException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected InvalidDateTimeFormatException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
