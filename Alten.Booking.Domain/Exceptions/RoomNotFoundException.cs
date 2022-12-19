using System.Runtime.Serialization;

namespace Alten.Booking.Domain.Exceptions
{
    public class RoomNotFoundException : ApplicationException
    {
        public RoomNotFoundException()
        {
        }

        public RoomNotFoundException(string? message) : base(message)
        {
        }

        public RoomNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoomNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
