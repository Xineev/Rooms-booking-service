using Microsoft.EntityFrameworkCore;
using RoomsBooking.Common;
using RoomsBooking.Data;
using RoomsBooking.DTO;
using RoomsBooking.Entities;
using RoomsBooking.Errors;

namespace RoomsBooking.Services;

public class BookingService(ApplicationDbContext _context) : IBookingService
{
    public async Task<Result<BookingResponse>> CreateBookingAsync(CreateBookingRequest request)
    {
        var validation = ValidateNewBooking(request);
        
        if(validation.IsFailure)
            return Result<BookingResponse>.Failure(validation.Error);
        
        var userSearch = await EnsureUserExistsAsync(request.UserId);
        
        if (userSearch.IsFailure)
            return Result<BookingResponse>.Failure(userSearch.Error);
        
        var roomSearch = await EnsureRoomExistsAsync(request.RoomId);

        if (roomSearch.IsFailure)
            return Result<BookingResponse>.Failure(roomSearch.Error);

        var overlapCheck = await EnsureNoOverlapAsync(request);
        
        if(overlapCheck.IsFailure)
            return Result<BookingResponse>.Failure(overlapCheck.Error);
        
        var booking = CreateNewBooking(request);
            
        _context.Bookings.Add(booking);
        
        await _context.SaveChangesAsync();

        return Result<BookingResponse>.Success(ToResponse(booking));
    }

    public async Task<List<BookingResponse>> GetAllBookingsAsync()
    {
        return await _context.Bookings
            .AsNoTracking()
            .Select(booking => ToResponse(booking))
            .ToListAsync();
    }

    public async Task<Result<BookingResponse>> GetBookingByIdAsync(int id)
    {
        var booking = await _context.Bookings
            .FindAsync(id);
        
        return booking is null ? Result<BookingResponse>.Failure(BookingErrors.NotFound) 
            : Result<BookingResponse>.Success(ToResponse(booking));
    }

    public async Task<Result<BookingResponse>> UpdateBookingAsync(int id, UpdateBookingRequest request)
    {
        var booking = await _context.Bookings
            .FindAsync(id);
        
        if(booking is null)
            return Result<BookingResponse>.Failure(BookingErrors.NotFound);
        
        if(booking.Status ==  BookingStatus.Completed)
            return Result<BookingResponse>.Failure(BookingErrors.BookingAlreadyCompleted);
        
        var requestForCreate = new CreateBookingRequest {
            RoomId = booking.RoomId,
            UserId = booking.UserId,
            EndTime  = request.EndTime,
            StartTime = request.StartTime
        };
        
        var validationResult = ValidateNewBooking(requestForCreate);
        
        if(validationResult.IsFailure)
            return Result<BookingResponse>.Failure(validationResult.Error);
        
        var overlapCheck = await EnsureNoOverlapForExistingBookingAsync(id, requestForCreate);
        
        if(overlapCheck.IsFailure)
            return Result<BookingResponse>.Failure(overlapCheck.Error);

        booking.UpdateTime(request.StartTime, request.EndTime);
        
        await _context.SaveChangesAsync();
        
        return Result<BookingResponse>.Success(ToResponse(booking));
    }

    public async Task<Result> DeleteBookingAsync(int id)
    {
        var booking = await _context.Bookings
            .FindAsync(id);
        
        if(booking is null)
            return Result<BookingResponse>.Failure(BookingErrors.NotFound);
        
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    private BookingResponse ToResponse(Booking booking)
    {
        return new BookingResponse()
        {
            Id = booking.Id,
            UserId = booking.UserId,
            RoomId = booking.RoomId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt,
            Status = booking.Status
        };
    }

    private Booking CreateNewBooking(CreateBookingRequest request)
    {
        return new Booking
        {
            RoomId = request.RoomId,
            UserId = request.UserId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = BookingStatus.Active
        };
    }

    private Result ValidateNewBooking(CreateBookingRequest request)
    {
        if(request.StartTime >= request.EndTime)
            return Result.Failure(BookingErrors.InvalidEndTime);
        
        if(request.StartTime < DateTime.UtcNow)
            return Result.Failure(BookingErrors.BookingInPast);

        return Result.Success();
    }

    private async Task<Result> EnsureUserExistsAsync(int userId)
    {
        var exists = await _context.Users.AnyAsync(u => u.Id == userId);
        
        if(exists is false)
            return Result.Failure(UserErrors.NotFound);
        
        return Result.Success();
    }

    private async Task<Result> EnsureRoomExistsAsync(int roomId)
    {
        var exists = await _context.Rooms.AnyAsync(r => r.Id == roomId);

        if (exists is false)
            return Result.Failure(RoomErrors.NotFound);
        
        return Result.Success();
    }

    private async Task<Result> EnsureNoOverlapAsync(CreateBookingRequest request)
    {
        var isOverlapping =  await _context.Bookings
            .AnyAsync(b =>
                b.RoomId == request.RoomId && 
                b.StartTime < request.EndTime &&
                b.EndTime > request.StartTime);

        if (isOverlapping)
            return Result.Failure(BookingErrors.BookingsOverlap);
        
        return Result.Success();
    }
    
    private async Task<Result> EnsureNoOverlapForExistingBookingAsync(int id, CreateBookingRequest request)
    {
        var isOverlapping =  await _context.Bookings
            .AnyAsync(b =>
                b.Id != id &&
                b.RoomId == request.RoomId && 
                b.StartTime < request.EndTime &&
                b.EndTime > request.StartTime);

        if (isOverlapping)
            return Result.Failure(BookingErrors.BookingsOverlap);
        
        return Result.Success();
    }
}