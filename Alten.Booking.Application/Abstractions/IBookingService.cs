using Alten.Booking.Domain.Model;

namespace Alten.Booking.Application.Abstractions
{
    public interface IBookingService
    {
        Task CancelReservationAsync(string reservationId);
        IEnumerable<Room> CheckRoomAvailability(DateTime desiredCheckin, DateTime desiredCheckout);
        Task<Reservation> ModifyReservationAsync(string reservationId, DateTime newCheckin, DateTime newCheckout);
        Task<Reservation> PlaceReservationAsync(Guest guest, int roomNumber, DateTime checkin, DateTime checkout);
        IEnumerable<Reservation> GetGuestReservations(string guestEmail);
        Task<Reservation?> GetReservationByIdAsync(string id);
    }
}
