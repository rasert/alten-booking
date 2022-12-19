using Alten.Booking.Domain.Model;

namespace Alten.Booking.Application.Abstractions
{
    public interface IBookingService
    {
        void CancelReservation(string reservationId);
        IEnumerable<Room> CheckRoomAvailability(DateTime desiredCheckin, DateTime desiredCheckout);
        void ModifyReservation(Reservation reservation);
        void PlaceReservation(Guest guest, int roomNumber, DateTime checkin, DateTime checkout);
    }
}
