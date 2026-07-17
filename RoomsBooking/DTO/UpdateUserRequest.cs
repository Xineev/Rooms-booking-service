using System.ComponentModel.DataAnnotations;

namespace RoomsBooking.DTO;

public class UpdateUserRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}