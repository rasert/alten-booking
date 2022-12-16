using Alten.Booking.Domain.Abstractions;

namespace Alten.Booking.Domain.Model
{
    public class Guest : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Guest()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
        }

        public Guest(string name, string phone, string email)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}
