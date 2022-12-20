using Alten.Booking.Domain.Abstractions;
using Alten.Booking.Domain.Model;
using System.Linq.Expressions;

namespace Alten.Booking.Domain.Queries
{
    public class AvailableRooms : IQuery<Room>
    {
        private readonly DateTime _desiredCheckin;
        private readonly DateTime _desiredCheckout;

        public AvailableRooms(DateTime desiredCheckin, DateTime desiredCheckout)
        {
            _desiredCheckin = desiredCheckin;
            _desiredCheckout = desiredCheckout;
        }

        public Expression<Func<Room, bool>> Criteria()
        {
            return r => r.Reservations.All(r => _desiredCheckin > r.Checkout || _desiredCheckout < r.Checkin);
        }
    }
}
