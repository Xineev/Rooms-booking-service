using RoomsBooking.Common;
using RoomsBooking.DTO;

namespace RoomsBooking.Services;

public interface IUserService
{
    public Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request);
    
    public Task<Result<UserResponse>> GetUserByIdAsync(int id);
    
    public Task<Result<UserResponse>> GetUserByEmailAsync(string email);
    
    public Task<List<UserResponse>> GetAllUsersAsync();
    
    public Task<Result<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest request);
    
    public Task<Result> DeleteUserAsync(int id);
}