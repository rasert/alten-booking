using Alten.Booking.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // TODO: validation -> the stay can’t be longer than 3 days
            // and can’t be reserved more than 30 days in advance.
            // All reservations start at least the next day of booking.

            // To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
        }
    }
}
