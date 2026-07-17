using RoomsBooking.Common;
using RoomsBooking.DTO;

namespace RoomsBooking.Services;

public interface IBookingService
{
    public Task<Result<BookingResponse>> CreateBookingAsync(CreateBookingRequest request);
    
    public Task<List<BookingResponse>> GetAllBookingsAsync();
    
    public Task<Result<BookingResponse>> GetBookingByIdAsync(int id);
    
    public Task<Result<List<BookingResponse>>> GetBookingsOfUserById(int id);

    public Task<Result<BookingResponse>> UpdateBookingAsync(int id, UpdateBookingRequest request);
    
    public Task<Result> DeleteBookingAsync(int id);
}