using Microsoft.AspNetCore.Mvc;
using RoomsBooking.Common;
using RoomsBooking.DTO;
using RoomsBooking.Errors;
using RoomsBooking.Services;

namespace RoomsBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        var userCreate = await userService.CreateUserAsync(request);
        
        return userCreate.IsFailure ? 
            ProcessError(userCreate.Error) : 
            CreatedAtAction(
                nameof(GetUserByIdAsync),
                new { id = userCreate.Value.Id }, 
                userCreate.Value);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var userUpdate = await userService.UpdateUserAsync(id, request);
        
        return userUpdate.IsFailure ? ProcessError(userUpdate.Error) :  Ok(userUpdate.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        var userDelete = await userService.DeleteUserAsync(id);

        return userDelete.IsFailure ? NotFound() : NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUserByIdAsync(int id)
    {
        var userById = await userService.GetUserByIdAsync(id);
        
        return userById.IsFailure ? ProcessError(userById.Error) : Ok(userById.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetUserByEmailAsync([FromQuery] string email)
    {
        var userByEmail = await userService.GetUserByEmailAsync(email);
        
        return userByEmail.IsFailure ? ProcessError(userByEmail.Error) : Ok(userByEmail.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAllUsersAsync()
    {
        return Ok(await userService.GetAllUsersAsync());
    }
    
    private ActionResult<UserResponse> ProcessError(Error error)
    {
        if (error == UserErrors.NotFound)
            return NotFound(error);

        return BadRequest(error);
    }
}