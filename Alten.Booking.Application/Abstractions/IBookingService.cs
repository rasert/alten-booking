using Alten.Booking.Domain.Model;

namespace Alten.Booking.Application.Abstractions
{
    public interface IBookingService
    {
        Task CancelReservationAsync(string reservationId);
        IEnumerable<Room> CheckRoomAvailability(DateTime desiredCheckin, DateTime desiredCheckout);
        Task ModifyReservationAsync(Reservation reservation);
        Task PlaceReservationAsync(Guest guest, int roomNumber, DateTime checkin, DateTime checkout);
    }
}
