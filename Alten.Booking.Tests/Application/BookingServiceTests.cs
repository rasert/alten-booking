using Alten.Booking.Application.Abstractions;
using Alten.Booking.Application.Services;
using Alten.Booking.Domain.Model;
using Alten.Booking.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace Alten.Booking.Tests.Application
{
    public class BookingServiceTests
    {
        private readonly BookingService _bookingService;
        private readonly List<Room> _rooms;
        private readonly List<Reservation> _reservations;

        public BookingServiceTests()
        {
            _rooms = new List<Room>();
            _reservations = new List<Reservation>();
            Mock<IRepository<Room>> roomRepoMock = MockHelpers.GetRepositoryMock<IRepository<Room>, Room>(_rooms);
            Mock<IRepository<Reservation>> reservationRepoMock = MockHelpers.GetRepositoryMock<IRepository<Reservation>, Reservation>(_reservations);
            Mock<IUnitOfWork> unitOfWorkMock = new();
            _bookingService = new BookingService(roomRepoMock.Object, reservationRepoMock.Object, unitOfWorkMock.Object);
        }

        [Fact]
        public void Should_ReturnAvailableRooms_When_AppropriatePeriodIsChecked()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _rooms.Add(new Room(number: 2, description: "Deluxe"));
            _reservations.Clear();
            _reservations.Add(_rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3)));
            _reservations.Add(_rooms[1].PlaceReservation(guest, checkin: DateTime.Now.AddDays(4), checkout: DateTime.Now.AddDays(6)));

            // act
            IEnumerable<Room> availableRooms = _bookingService.CheckRoomAvailability(
                desiredCheckin: DateTime.Now.AddHours(30),
                desiredCheckout: DateTime.Now.AddDays(3));

            // assert
            availableRooms.Should().HaveCount(1);
        }

        [Fact]
        public void Should_NotReturnAvailableRooms_When_BusyPeriodIsChecked()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _rooms.Add(new Room(number: 2, description: "Deluxe"));
            _reservations.Clear();
            _reservations.Add(_rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3)));
            _reservations.Add(_rooms[1].PlaceReservation(guest, checkin: DateTime.Now.AddDays(2), checkout: DateTime.Now.AddDays(4)));

            // act
            IEnumerable<Room> availableRooms = _bookingService.CheckRoomAvailability(
                desiredCheckin: DateTime.Now.AddHours(30),
                desiredCheckout: DateTime.Now.AddDays(3));

            // assert
            availableRooms.Should().HaveCount(0);
        }
    }
}
