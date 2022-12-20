using Alten.Booking.Application.Abstractions;
using Alten.Booking.Application.Services;
using Alten.Booking.Domain.Exceptions;
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

        [Fact]
        public async Task Should_PlaceReservation_When_RoomIsAvailableAtDesiredPeriod()
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
            Func<Task> act = async () => await _bookingService.PlaceReservationAsync(
                guest, roomNumber: 1,
                checkin: DateTime.Now.AddDays(4),
                checkout: DateTime.Now.AddDays(6));

            // assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Should_NotPlaceReservation_When_RoomIsNotAvailableAtDesiredPeriod()
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
            Func<Task> act = async () => await _bookingService.PlaceReservationAsync(
                guest, roomNumber: 1,
                checkin: DateTime.Now.AddDays(2),
                checkout: DateTime.Now.AddDays(4));

            // assert
            await act.Should().ThrowAsync<PeriodNotAvailableException>();
        }

        [Fact]
        public async Task Should_NotPlaceReservation_When_RoomIsNotFound()
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
            Func<Task> act = async () => await _bookingService.PlaceReservationAsync(
                guest, roomNumber: 99,
                checkin: DateTime.Now.AddDays(2),
                checkout: DateTime.Now.AddDays(4));

            // assert
            await act.Should().ThrowAsync<RoomNotFoundException>();
        }

        [Fact]
        public async Task Should_CancelReservation_When_ReservationIsFound()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.CancelReservationAsync(reservationId);

            // assert
            await act.Should().NotThrowAsync();
            _reservations.Should().HaveCount(0);
        }

        [Fact]
        public async Task Should_NotCancelReservation_When_ReservationIsNotFound()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.CancelReservationAsync("wrong-id");

            // assert
            await act.Should().ThrowAsync<ReservationNotFoundException>();
        }

        [Fact]
        public async Task Should_ModifyReservation_When_NewPeriodIsAvailable()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddDays(7), checkout: DateTime.Now.AddDays(9));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.ModifyReservationAsync(
                reservationId, newCheckin: DateTime.Now.AddDays(4), newCheckout: DateTime.Now.AddDays(6));

            // assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Should_NotModifyReservation_When_NewPeriodIsNotAvailable()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddDays(7), checkout: DateTime.Now.AddDays(9));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.ModifyReservationAsync(
                reservationId, newCheckin: DateTime.Now.AddDays(6), newCheckout: DateTime.Now.AddDays(8));

            // assert
            await act.Should().ThrowAsync<PeriodNotAvailableException>();
        }

        [Fact]
        public async Task Should_NotModifyReservation_When_NewPeriodIsTooLong()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.ModifyReservationAsync(
                reservationId, newCheckin: DateTime.Now.AddDays(2), newCheckout: DateTime.Now.AddDays(15));

            // assert
            await act.Should().ThrowAsync<TooLongStayException>();
        }

        [Fact]
        public async Task Should_NotModifyReservation_When_ReservationIsNotFound()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddDays(7), checkout: DateTime.Now.AddDays(9));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.ModifyReservationAsync(
                "wrong-id", newCheckin: DateTime.Now.AddDays(4), newCheckout: DateTime.Now.AddDays(6));

            // assert
            await act.Should().ThrowAsync<ReservationNotFoundException>();
        }

        [Fact]
        public async Task Should_NotModifyReservation_When_LeadTimeIsTooShort()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddDays(7), checkout: DateTime.Now.AddDays(9));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.ModifyReservationAsync(
                reservationId, newCheckin: DateTime.Now, newCheckout: DateTime.Now.AddDays(2));

            // assert
            await act.Should().ThrowAsync<InvalidLeadTimeException>();
        }

        [Fact]
        public async Task Should_NotModifyReservation_When_LeadTimeIsTooFarAway()
        {
            // arrange
            Guest guest = new(name: "Peter Quill", phone: "6781238734", email: "starlord@gmail.com");
            const string reservationId = "76365025-0AA0-4323-962F-F02C312BB6C2";
            _rooms.Clear();
            _rooms.Add(new Room(number: 1, description: "Standard"));
            _reservations.Clear();
            var reservation = _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddHours(30), checkout: DateTime.Now.AddDays(3));
            _rooms[0].PlaceReservation(guest, checkin: DateTime.Now.AddDays(7), checkout: DateTime.Now.AddDays(9));
            reservation.Id = reservationId;
            _reservations.Add(reservation);

            // act
            Func<Task> act = async () => await _bookingService.ModifyReservationAsync(
                reservationId, newCheckin: DateTime.Now.AddDays(40), newCheckout: DateTime.Now.AddDays(42));

            // assert
            await act.Should().ThrowAsync<InvalidLeadTimeException>();
        }
    }
}
