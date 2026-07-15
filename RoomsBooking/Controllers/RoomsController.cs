using Microsoft.AspNetCore.Mvc;
using RoomsBooking.DTO;
using RoomsBooking.Entities;
using RoomsBooking.Services;

namespace RoomsBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(IRoomService _roomService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RoomResponse>> CreateRoomAsync(CreateRoomRequest request)
    {
        var room = await _roomService.CreateRoomAsync(request);

        return CreatedAtAction(
            nameof(GetRoomById),
            new { id = room.Id },
            room);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomResponse>> GetRoomById(int id)
    {
        var result = await _roomService.GetRoomByIdAsync(id);

        if (result.IsFailure) return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoomResponse>>> GetAllRooms()
    {
        var result = await _roomService.GetAllRoomsAsync();
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoomResponse>> UpdateRoom(int id, CreateRoomRequest request)
    {
        var result = await _roomService.UpdateRoomAsync(id, request);
        
        if (result.IsFailure) return NotFound(result.Error);
        
        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<RoomResponse>> DeleteRoom(int id)
    {
        var result = await _roomService.DeleteRoomAsync(id);
        
        if (result.IsFailure) return NotFound(result.Error);
        
        return Ok(result);
    }
}