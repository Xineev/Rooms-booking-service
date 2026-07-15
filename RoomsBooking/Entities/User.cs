namespace RoomsBooking.Entities;

public class User
{
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}