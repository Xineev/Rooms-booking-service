using System.ComponentModel.DataAnnotations;

namespace RoomsBooking.DTO;

public class CreateRoomRequest
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; } =  string.Empty;
    
    [Required]
    [Range(1, 100)]
    public int Capacity { get; set; }
}