using Alten.Booking.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Alten.Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public RoomController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/<RoomsController>
        [HttpGet("available")]
        public IActionResult Get([Required][FromQuery] DateTime checkin, [Required][FromQuery] DateTime checkout)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_bookingService.CheckRoomAvailability(checkin, checkout));
        }
    }
}
