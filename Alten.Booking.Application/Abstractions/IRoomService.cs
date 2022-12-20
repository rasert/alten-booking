using Alten.Booking.Domain.Model;

namespace Alten.Booking.Application.Abstractions
{
    public interface IRoomService
    {
        IEnumerable<Room> CheckRoomAvailability(DateTime desiredCheckin, DateTime desiredCheckout);
    }
}
