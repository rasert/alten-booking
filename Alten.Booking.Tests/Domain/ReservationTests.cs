using Alten.Booking.Domain.Exceptions;
using Alten.Booking.Domain.Model;
using FluentAssertions;

namespace Alten.Booking.Tests.Domain
{
    public class ReservationTests
    {
        [Fact]
        public void Should_ThrowException_When_StayIsLongerThan3Days()
        {
            // arrange
            Guest guest = new (name: "Tony Stark", phone: "3459879514", email: "ironman@starkindustries.com");
            Room room = new (1005, "Expensive Rooftop");
            DateTime checkin = DateTime.Now.AddDays(1);
            DateTime checkout = checkin.AddDays(4);

            // act
            Action act = () => _ = new Reservation(guest, room, checkin, checkout);

            // assert
            act.Should().Throw<TooLongStayException>();
        }

        [Fact]
        public void Should_ThrowException_When_CheckinIsOnTheSameDayAsTheReservation()
        {
            // arrange
            Guest guest = new(name: "Tony Stark", phone: "3459879514", email: "ironman@starkindustries.com");
            Room room = new(1005, "Expensive Rooftop");
            DateTime checkin = DateTime.Now;
            DateTime checkout = checkin.AddDays(2);

            // act
            Action act = () => _ = new Reservation(guest, room, checkin, checkout);

            // assert
            act.Should().Throw<InvalidLeadTimeException>();
        }

        [Fact]
        public void Should_ThrowException_When_ReservationIsMadeMoreThan30DaysInAdvance()
        {
            // arrange
            Guest guest = new(name: "Tony Stark", phone: "3459879514", email: "ironman@starkindustries.com");
            Room room = new(1005, "Expensive Rooftop");
            DateTime checkin = DateTime.Now.AddDays(40);
            DateTime checkout = checkin.AddDays(2);

            // act
            Action act = () => _ = new Reservation(guest, room, checkin, checkout);

            // assert
            act.Should().Throw<InvalidLeadTimeException>();
        }
    }
}
