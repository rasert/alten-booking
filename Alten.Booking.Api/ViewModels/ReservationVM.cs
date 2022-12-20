namespace Alten.Booking.Api.ViewModels
{
    public class ReservationVM
    {
        public GuestVM Guest { get; set; }
        public int RoomNumber { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public ReservationVM()
        {
            Guest = new GuestVM();
        }
    }
}
