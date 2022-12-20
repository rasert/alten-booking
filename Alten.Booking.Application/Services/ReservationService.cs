using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Exceptions;
using Alten.Booking.Domain.Model;
using System.Linq.Expressions;

namespace Alten.Booking.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Room> _rooms;
        private readonly IRepository<Reservation> _reservations;
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(
            IRepository<Room> rooms,
            IRepository<Reservation> reservations,
            IUnitOfWork unitOfWork)
        {
            _rooms = rooms;
            _reservations = reservations;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Reservation> GetGuestReservations(string guestEmail)
        {
            if (string.IsNullOrEmpty(guestEmail))
                throw new ArgumentNullException(nameof(guestEmail));

            return _reservations.Get(
                expression: r => r.Guest.Email.Equals(guestEmail),
                includes: new Expression<Func<Reservation, object>>[] { r => r.Room });
        }

        public async Task<Reservation?> GetReservationByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return await _reservations.GetAsync(id,
                includes: new Expression<Func<Reservation, object>>[] { r => r.Room });
        }

        public async Task<Reservation> PlaceReservationAsync(Guest guest, int roomNumber, DateTime checkin, DateTime checkout)
        {
            if (guest == null)
                throw new ArgumentNullException(nameof(guest));
            if (roomNumber <= 0)
                throw new ArgumentException("Room number must be greater than zero.", nameof(roomNumber));
            if (checkin > checkout)
                throw new ArgumentOutOfRangeException(nameof(checkin), "Checkin date cannot be greater than checkout date.");

            Room? room = _rooms.Get(r => r.Number == roomNumber).SingleOrDefault();
            if (room == null)
                throw new RoomNotFoundException();

            Reservation newReservation = room.PlaceReservation(guest, checkin, checkout);

            await _unitOfWork.SaveChangesAsync();

            return newReservation;
        }

        public async Task CancelReservationAsync(string reservationId)
        {
            if (string.IsNullOrEmpty(reservationId))
                throw new ArgumentNullException(nameof(reservationId));

            Reservation? reservation = await _reservations.GetAsync(id: reservationId);

            if (reservation == null)
                throw new ReservationNotFoundException();

            _reservations.Remove(reservation);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Reservation> ModifyReservationAsync(string reservationId, DateTime newCheckin, DateTime newCheckout)
        {
            if (string.IsNullOrEmpty(reservationId))
                throw new ArgumentNullException(nameof(reservationId));
            if (newCheckin > newCheckout)
                throw new ArgumentOutOfRangeException(nameof(newCheckin), "Checkin date cannot be greater than checkout date.");

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
