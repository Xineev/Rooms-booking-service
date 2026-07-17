using System.ComponentModel.DataAnnotations;

namespace RoomsBooking.DTO;

public class CreateUserRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}