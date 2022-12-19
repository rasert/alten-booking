using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // TODO
            _rooms = rooms;
            _reservations = reservations;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Room> CheckRoomAvailability(DateTime desiredCheckin, DateTime desiredCheckout)
        {
            throw new NotImplementedException();
        }

        public void PlaceReservation(Guest guest, Room room, DateTime checkin, DateTime checkout)
        {
            // TODO: validate room availability
            throw new NotImplementedException();
        }

        public void CancelReservation(string reservationId)
        {
            throw new NotImplementedException();
        }

        public void ModifyReservation(Reservation reservation)
        {
            // TODO: validate room availability
            throw new NotImplementedException();
        }
    }
}
