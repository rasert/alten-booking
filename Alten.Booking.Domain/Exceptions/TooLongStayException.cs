using System.Runtime.Serialization;

namespace Alten.Booking.Domain.Exceptions
{
    public class TooLongStayException : ApplicationException
    {
        public TooLongStayException()
        {
        }

        public TooLongStayException(string? message) : base(message)
        {
        }

        public TooLongStayException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TooLongStayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
