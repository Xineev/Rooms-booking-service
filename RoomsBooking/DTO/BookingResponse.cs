using RoomsBooking.Entities;

namespace RoomsBooking.DTO;

public class BookingResponse
{
    public int Id { get; set; }
    
    public int RoomId { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public BookingStatus Status { get; set; }
}