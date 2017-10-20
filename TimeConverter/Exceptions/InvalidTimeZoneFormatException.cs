using System;
using System.Runtime.Serialization;

namespace TimeConverter.Exceptions
{
    [Serializable]
    public class InvalidTimeZoneFormatException : TimeConverterException
    {
        public InvalidTimeZoneFormatException()
        {
        }

        public InvalidTimeZoneFormatException(string message) 
            : base(message)
        {
        }

        public InvalidTimeZoneFormatException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected InvalidTimeZoneFormatException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
