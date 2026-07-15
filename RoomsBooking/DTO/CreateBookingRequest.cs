using System.ComponentModel.DataAnnotations;

namespace RoomsBooking.DTO;

public class CreateBookingRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int RoomId { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }
}