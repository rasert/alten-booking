using Alten.Booking.Api.ViewModels;
using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Exceptions;
using Alten.Booking.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Alten.Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public ReservationController(IBookingService bookingService)
        {
            // TODO: Exception Handling and Status Codes

            _bookingService = bookingService;
        }

        // GET: api/<ReservationsController>
        [HttpGet]
        public IActionResult Get([Required][FromQuery] string guestEmail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<Reservation> reservations = _bookingService.GetGuestReservations(guestEmail);

            return Ok(reservations);
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id must not be null or empty.");

            Reservation? reservation = await _bookingService.GetReservationByIdAsync(id);

            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        // POST api/<ReservationsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservationVM reservationVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Reservation newReservation = await _bookingService.PlaceReservationAsync(
                reservationVM.Guest.AsModel(),
                reservationVM.RoomNumber,
                reservationVM.CheckIn,
                reservationVM.CheckOut);

            return CreatedAtAction(nameof(GetById), new { id = newReservation.Id }, newReservation);
        }

        // PUT api/<ReservationsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ModifyReservationVM modifyReservationVM)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id must not be null or empty.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Reservation modifiedReservation = await _bookingService.ModifyReservationAsync(
                    id, modifyReservationVM.CheckIn, modifyReservationVM.CheckOut);

                return Ok(modifiedReservation);
            }
            catch (ReservationNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/<ReservationsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id must not be null or empty.");

            try
            {
                await _bookingService.CancelReservationAsync(id);

                return NoContent();
            }
            catch (ReservationNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
