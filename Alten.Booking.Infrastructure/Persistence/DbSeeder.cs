using Alten.Booking.Domain.Model;

namespace Alten.Booking.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationContext context)
        {
            if (context.Rooms.Any() || context.Reservations.Any() || context.Guests.Any())
            {
                return; // DB has been seeded already
            }

            var guests = new Guest[]
            {
                new Guest(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com"),
                new Guest(name: "Peter Parker", phone: "3129781726", email: "spiderman@hotmail.com"),
                new Guest(name: "Tony Stark", phone: "3459879514", email: "ironman@starkindustries.com")
            };
            context.Guests.AddRange(guests);
            context.SaveChanges();

            var rooms = new Room[]
            {
                new Room(number: 101, description: "Standard"),
                new Room(number: 102, description: "Standard"),
                new Room(number: 201, description: "Deluxe"),
                new Room(number: 202, description: "Deluxe"),
            };
            context.Rooms.AddRange(rooms);
            context.SaveChanges();

            rooms[0].PlaceReservation(guests[0], checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            rooms[1].PlaceReservation(guests[1], checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            rooms[2].PlaceReservation(guests[2], checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            context.SaveChanges();
        }
    }
}
