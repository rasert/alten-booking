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

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(phone))
                throw new ArgumentNullException(nameof(phone));

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
        }
    }
}
