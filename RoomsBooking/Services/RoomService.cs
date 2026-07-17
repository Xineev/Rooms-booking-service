using Microsoft.EntityFrameworkCore;
using RoomsBooking.Data;
using RoomsBooking.DTO;
using RoomsBooking.Entities;
using RoomsBooking.Common;
using RoomsBooking.Errors;

namespace RoomsBooking.Services;

public class RoomService(ApplicationDbContext _context) : IRoomService
{

    public async Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request)
    {
        var room = new Room
        {
            Name = request.Name, 
            Capacity = request.Capacity
        };
        
        _context.Rooms.Add(room);

        await _context.SaveChangesAsync();

        return ToResponse(room);
    }

    public async Task<Result<RoomResponse>> GetRoomByIdAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room is null) return Result<RoomResponse>.Failure(RoomErrors.NotFound);

        return Result<RoomResponse>.Success(ToResponse(room));
    }

    public async Task<List<RoomResponse>> GetAllRoomsAsync()
    {
        return await _context.Rooms
            .AsNoTracking()
            .Select(room => ToResponse(room))
            .ToListAsync();
    }

    public async Task<Result<RoomResponse>> DeleteRoomAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room is null) return Result<RoomResponse>.Failure(RoomErrors.NotFound);

        _context.Rooms.Remove(room);
        
        await _context.SaveChangesAsync();

        return Result<RoomResponse>.Success(ToResponse(room));
    }

    public async Task<Result<RoomResponse>> UpdateRoomAsync(int id, CreateRoomRequest request)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room is null) return Result<RoomResponse>.Failure(RoomErrors.NotFound);
        
        room.Name = request.Name;
        room.Capacity = request.Capacity;

        await _context.SaveChangesAsync();

        return Result<RoomResponse>.Success(ToResponse(room));
    }

    private static RoomResponse ToResponse(Room room)
    {
        return new RoomResponse
        {
            Id = room.Id, 
            Capacity = room.Capacity, 
            Name = room.Name
        };
    }
}