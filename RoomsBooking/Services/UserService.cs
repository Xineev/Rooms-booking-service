using Microsoft.EntityFrameworkCore;
using RoomsBooking.Common;
using RoomsBooking.Data;
using RoomsBooking.DTO;
using RoomsBooking.Entities;
using RoomsBooking.Errors;

namespace RoomsBooking.Services;

public class UserService(ApplicationDbContext _context) : IUserService
{
    public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        var user = ToUser(request);
            
        var userValidation = await ValidateNewUser(user);

        if (userValidation.IsFailure)
            return Result<UserResponse>.Failure(userValidation.Error);
        
        _context.Users.Add(user);
        
        await _context.SaveChangesAsync();
        
        return Result<UserResponse>.Success(ToResponse(user));
    }

    public async Task<Result<UserResponse>> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if(user == null)
            return Result<UserResponse>.Failure(UserErrors.NotFound);
        
        return Result<UserResponse>.Success(ToResponse(user));
    }

    public async Task<Result<UserResponse>> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if(user == null)
            return Result<UserResponse>.Failure(UserErrors.NotFound);
        
        return Result<UserResponse>.Success(ToResponse(user));
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Select(u => ToResponse(u))
            .ToListAsync();
        
        return users;
    }

    public async Task<Result<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var userToUpdate = await _context.Users.FindAsync(id);
        
        if(userToUpdate == null)
            return Result<UserResponse>.Failure(UserErrors.NotFound);
        
        userToUpdate.Login = request.Login;
        userToUpdate.Password = request.Password;
        userToUpdate.Email = request.Email;

        await _context.SaveChangesAsync();
        
        return Result<UserResponse>.Success(ToResponse(userToUpdate));
    }

    public async Task<Result> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id); 
        
        if(user == null)
            return Result<UserResponse>.Failure(UserErrors.NotFound);
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }
    
    private static UserResponse ToResponse(User user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email
        };
    }

    private static User ToUser(CreateUserRequest createUserRequest)
    {
        return new User
        {
            Login = createUserRequest.Login,
            Password = createUserRequest.Password,
            Email = createUserRequest.Email
        };
    }
    
    private async Task<Result> ValidateNewUser(User request)
    { 
        var isAlreadyExists = await _context.Users
            .AnyAsync(u => 
                u.Login == request.Login &&
                u.Email == request.Email);
        
        if (isAlreadyExists)
            return Result.Failure(UserErrors.AlreadyExists);
        
        return Result.Success();
    }
}