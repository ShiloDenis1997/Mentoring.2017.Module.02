using System;
using System.Runtime.Serialization;

namespace TimeConverter.Exceptions
{
    [Serializable]
    public class TimeConverterException : Exception
    {
        public TimeConverterException()
        {
        }

        public TimeConverterException(string message)
            : base(message)
        {
        }

        public TimeConverterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TimeConverterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
