using Alten.Booking.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alten.Booking.Domain.Model
{
    public class Room : IEntity
    {
        public string Id { get; set; }
        public virtual List<Reservation> Reservations { get; set; }

        public Room()
        {
            Id = Guid.NewGuid().ToString();
            Reservations = new List<Reservation>();
        }
    }
}
