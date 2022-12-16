using Alten.Booking.Domain.Abstractions;

namespace Alten.Booking.Domain.Model
{
    public class Room : IEntity
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public virtual List<Reservation> Reservations { get; set; }

        // Room Capacity: for simplicity, it won't be done.

        public Room()
        {
            Id = Guid.NewGuid().ToString();
            Number = 0;
            Description = string.Empty;
            Reservations = new List<Reservation>();            
        }

        public Room(int number, string description)
        {
            Id = Guid.NewGuid().ToString();
            Number = number;
            Description = description;
            Reservations = new List<Reservation>();
        }
    }
}
