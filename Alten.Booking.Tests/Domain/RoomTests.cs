using Alten.Booking.Domain.Exceptions;
using Alten.Booking.Domain.Model;
using FluentAssertions;

namespace Alten.Booking.Tests.Domain
{
    public class RoomTests
    {
        [Fact]
        public void Should_BeAvailable_When_ThereAreNoReservationsInTheDesiredPeriod()
        {
            // arrange
            var guest = new Guest(name: "Peter Parker", phone: "3129781726", email: "spiderman@hotmail.com");
            var room = new Room(number: 1, description: "Low Cost");
            List<Reservation> reservations = new()
            {
                new Reservation(guest, room, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3)),
                new Reservation(guest, room, checkin: DateTime.Now.AddDays(7), checkout: DateTime.Now.AddDays(9))
            };
            room.Reservations = reservations;

            // act
            var result = room.IsAvailable(desiredCheckin: DateTime.Now.AddDays(4), desiredCheckout: DateTime.Now.AddDays(6));

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_NotBeAvailable_When_ThereAreReservationsInTheDesiredPeriod()
        {
            // arrange
            var guest = new Guest(name: "Peter Parker", phone: "3129781726", email: "spiderman@hotmail.com");
            var room = new Room(number: 1, description: "Low Cost");
            List<Reservation> reservations = new()
            {
                new Reservation(guest, room, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3)),
                new Reservation(guest, room, checkin: DateTime.Now.AddDays(4), checkout: DateTime.Now.AddDays(6))
            };
            room.Reservations = reservations;

            // act
            var result = room.IsAvailable(desiredCheckin: DateTime.Now.AddDays(2), desiredCheckout: DateTime.Now.AddDays(5));

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_PlaceReservation_When_ThereAreNoReservationsInTheDesiredPeriod()
        {
            // arrange
            var guest = new Guest(name: "Peter Parker", phone: "3129781726", email: "spiderman@hotmail.com");
            var room = new Room(number: 1, description: "Low Cost");
            List<Reservation> reservations = new()
            {
                new Reservation(guest, room, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3)),
                new Reservation(guest, room, checkin: DateTime.Now.AddDays(7), checkout: DateTime.Now.AddDays(9))
            };
            room.Reservations = reservations;

            // act
            room.PlaceReservation(guest, checkin: DateTime.Now.AddDays(4), checkout: DateTime.Now.AddDays(6));

            // assert
            room.Reservations.Count.Should().Be(3);
        }

        [Fact]
        public void Should_NotPlaceReservation_When_ThereAreReservationsInTheDesiredPeriod()
        {
            // arrange
            var guest = new Guest(name: "Peter Parker", phone: "3129781726", email: "spiderman@hotmail.com");
            var room = new Room(number: 1, description: "Low Cost");
            List<Reservation> reservations = new()
            {
                new Reservation(guest, room, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3)),
                new Reservation(guest, room, checkin: DateTime.Now.AddDays(4), checkout: DateTime.Now.AddDays(6))
            };
            room.Reservations = reservations;

            // act
            Action act = () => room.PlaceReservation(guest, checkin: DateTime.Now.AddDays(2), checkout: DateTime.Now.AddDays(5));

            // assert
            act.Should().Throw<PeriodNotAvailableException>();
        }
    }
}
