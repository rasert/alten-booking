using Alten.Booking.Domain.Abstractions;
using Alten.Booking.Domain.Exceptions;

namespace Alten.Booking.Domain.Model
{
    public class Reservation : IEntity
    {
        public string Id { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual Room Room { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }

        public Reservation()
        {
            Id = Guid.NewGuid().ToString();

            // To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
        }

        public Reservation(Guest guest, Room room, DateTime checkin, DateTime checkout)
        {
            Id = Guid.NewGuid().ToString();
            Guest = guest;
            Room = room;
            Checkin = checkin;
            Checkout = checkout;

            if (Checkout - Checkin > TimeSpan.FromDays(3))
                throw new TooLongStayException();

            TimeSpan leadTime = Checkin - DateTime.Now;
            if (leadTime < TimeSpan.FromDays(1) || leadTime > TimeSpan.FromDays(30))
                throw new InvalidLeadTimeException();

            // TODO: checkin cannot be greater than checkout
        }
    }
}
