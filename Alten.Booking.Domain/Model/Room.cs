﻿using Alten.Booking.Domain.Abstractions;
using System.Xml.Linq;

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

            if (number <= 0)
                throw new ArgumentException("Room number must be above zero.", nameof(number));

            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));
        }

        public bool IsAvailable(DateTime desiredCheckin, DateTime desiredCheckout)
        {
            throw new NotImplementedException();
        }
    }
}
