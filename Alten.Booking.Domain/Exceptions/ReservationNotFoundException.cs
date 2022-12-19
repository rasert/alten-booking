using System.Runtime.Serialization;

namespace Alten.Booking.Domain.Exceptions
{
    public class ReservationNotFoundException : ApplicationException
    {
        public ReservationNotFoundException()
        {
        }

        public ReservationNotFoundException(string? message) : base(message)
        {
        }

        public ReservationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReservationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
