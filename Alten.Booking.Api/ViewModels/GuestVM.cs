using Alten.Booking.Domain.Model;

namespace Alten.Booking.Api.ViewModels
{
    public class GuestVM
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public GuestVM()
        {
            Name = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
        }

        public Guest AsModel()
        {
            return new Guest(Name, Phone, Email);
        }
    }
}
