using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Exceptions;
using Alten.Booking.Domain.Model;
using Alten.Booking.Domain.Queries;
using System.Linq.Expressions;

namespace Alten.Booking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Room> _rooms;
        private readonly IRepository<Reservation> _reservations;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(
            IRepository<Room> rooms,
            IRepository<Reservation> reservations,
            IUnitOfWork unitOfWork)
        {
            // TODO: rename to ReservationService
            _rooms = rooms;
            _reservations = reservations;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Room> CheckRoomAvailability(DateTime desiredCheckin, DateTime desiredCheckout)
        {
            // TODO: move to RoomService
            return _rooms.Get(new AvailableRooms(desiredCheckin, desiredCheckout));
        }

        public IEnumerable<Reservation> GetGuestReservations(string guestEmail)
        {
            // TODO: create IQuery
            return _reservations.Get(
                expression: r => r.Guest.Email.Equals(guestEmail),
                includes: new Expression<Func<Reservation, object>>[] { r => r.Room });
        }

        public async Task<Reservation?> GetReservationByIdAsync(string id)
        {
            return await _reservations.GetAsync(id,
                includes: new Expression<Func<Reservation, object>>[] { r => r.Room });
        }

        public async Task<Reservation> PlaceReservationAsync(Guest guest, int roomNumber, DateTime checkin, DateTime checkout)
        {
            Room? room = _rooms.Get(r => r.Number == roomNumber).SingleOrDefault();
            if (room == null)
                throw new RoomNotFoundException();

            Reservation newReservation = room.PlaceReservation(guest, checkin, checkout);

            await _unitOfWork.SaveChangesAsync();

            return newReservation;
        }

        public async Task CancelReservationAsync(string reservationId)
        {
            Reservation? reservation = await _reservations.GetAsync(id: reservationId);

            if (reservation == null)
                throw new ReservationNotFoundException();

            _reservations.Remove(reservation);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Reservation> ModifyReservationAsync(string reservationId, DateTime newCheckin, DateTime newCheckout)
        {
            Reservation? reservation = await _reservations.GetAsync(
                id: reservationId,
                includes: new Expression<Func<Reservation, object>>[] { r => r.Room, r => r.Guest }); ;

            if (reservation == null)
                throw new ReservationNotFoundException();

            Room room = reservation.Room;
            Guest guest = reservation.Guest;

            _reservations.Remove(reservation);
            room.Reservations.Remove(reservation);

            Reservation modifiedReservation = room.PlaceReservation(guest, newCheckin, newCheckout);

            await _unitOfWork.SaveChangesAsync();

            return modifiedReservation;
        }
    }
}
