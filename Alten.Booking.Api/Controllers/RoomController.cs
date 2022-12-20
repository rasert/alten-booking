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
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/<RoomsController>
        [HttpGet("available")]
        public IActionResult Get([Required][FromQuery] DateTime checkin, [Required][FromQuery] DateTime checkout)
        {
            if (checkin > checkout)
                ModelState.AddModelError(key: "InvalidPeriod", errorMessage: "Checkin date cannot be greater than checkout date.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_roomService.CheckRoomAvailability(checkin, checkout));
        }
    }
}
