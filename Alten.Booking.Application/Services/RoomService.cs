using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Model;
using Alten.Booking.Domain.Queries;

namespace Alten.Booking.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> _rooms;

        public RoomService(IRepository<Room> rooms)
        {
            _rooms = rooms;
        }

        public IEnumerable<Room> CheckRoomAvailability(DateTime desiredCheckin, DateTime desiredCheckout)
        {
            return _rooms.Get(new AvailableRooms(desiredCheckin, desiredCheckout));
        }
    }
}
