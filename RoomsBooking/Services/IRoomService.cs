using RoomsBooking.Common;
using RoomsBooking.DTO;

namespace RoomsBooking.Services;

public interface IRoomService
{
    public Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request);

    public Task<Result<RoomResponse>> GetRoomByIdAsync(int id);

    public Task<List<RoomResponse>> GetAllRoomsAsync();
    
    public Task<Result<RoomResponse>> DeleteRoomAsync(int id);
    
    public Task<Result<RoomResponse>> UpdateRoomAsync(int id, CreateRoomRequest request);
}