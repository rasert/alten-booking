using System.Runtime.Serialization;

namespace Alten.Booking.Domain.Exceptions
{
    public class InvalidLeadTimeException : ApplicationException
    {
        public InvalidLeadTimeException()
        {
        }

        public InvalidLeadTimeException(string? message) : base(message)
        {
        }

        public InvalidLeadTimeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidLeadTimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
