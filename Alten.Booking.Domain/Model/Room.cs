using Alten.Booking.Domain.Abstractions;

namespace Alten.Booking.Domain.Model
{
    public class Room : IEntity
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public virtual List<Reservation> Reservations { get; set; }

        public Room()
        {
            Id = Guid.NewGuid().ToString();
            Reservations = new List<Reservation>();
            Description = string.Empty;
        }

        public Room(string description)
        {
            Id = Guid.NewGuid().ToString();
            Reservations = new List<Reservation>();
            Description = description;
        }
    }
}
