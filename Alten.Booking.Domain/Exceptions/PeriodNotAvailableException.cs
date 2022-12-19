using System.Runtime.Serialization;

namespace Alten.Booking.Domain.Exceptions
{
    public class PeriodNotAvailableException : ApplicationException
    {
        public PeriodNotAvailableException()
        {
        }

        public PeriodNotAvailableException(string? message) : base(message)
        {
        }

        public PeriodNotAvailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PeriodNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
