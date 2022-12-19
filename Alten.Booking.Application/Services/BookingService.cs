﻿using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Exceptions;
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
            return _rooms.Get(r => r.IsAvailable(desiredCheckin, desiredCheckout));
        }

        public async Task PlaceReservationAsync(Guest guest, int roomNumber, DateTime checkin, DateTime checkout)
        {
            Room? room = _rooms.Get(r => r.Number == roomNumber).SingleOrDefault();
            if (room == null)
                throw new RoomNotFoundException();

            room.PlaceReservation(guest, checkin, checkout);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelReservationAsync(string reservationId)
        {
            throw new NotImplementedException();
        }

        public async Task ModifyReservationAsync(Reservation reservation)
        {
            // TODO: validate room availability
            throw new NotImplementedException();
        }
    }
}
