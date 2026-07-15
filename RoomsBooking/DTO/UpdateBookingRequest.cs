using System.ComponentModel.DataAnnotations;

namespace RoomsBooking.DTO;

public class UpdateBookingRequest
{
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }
}