using Microsoft.AspNetCore.Mvc;
using RoomsBooking.Common;
using RoomsBooking.DTO;
using RoomsBooking.Errors;
using RoomsBooking.Services;

namespace RoomsBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<BookingResponse>> CreateBookingAsync(CreateBookingRequest request)
    {
        var createResult = await bookingService.CreateBookingAsync(request);
        
        return createResult.IsFailure ? ProcessError(createResult.Error) : 
            CreatedAtAction(nameof(GetBookingAsync), 
                new { id = createResult.Value.Id }, 
                createResult.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingResponse>> GetBookingAsync(int id)
    {
        var booking = await bookingService.GetBookingByIdAsync(id);
        
        return booking.IsFailure ? ProcessError(booking.Error) : Ok(booking.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<BookingResponse>>> GetAllBookingsAsync()
    {
        var bookings = await bookingService.GetAllBookingsAsync();
        
        return Ok(bookings);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookingAsync(int id)
    {
        var bookingDelete = await bookingService.DeleteBookingAsync(id); 
        
        return bookingDelete.IsFailure ? NotFound(bookingDelete.Error) : NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookingResponse>> UpdateBookingAsync(int id, UpdateBookingRequest request)
    {
        var bookingUpdate = await bookingService.UpdateBookingAsync(id, request);
        
        return bookingUpdate.IsFailure ?  ProcessError(bookingUpdate.Error) : Ok(bookingUpdate.Value);
    }

    private ActionResult<BookingResponse> ProcessError(Error error)
    {
        if(error == BookingErrors.NotFound 
           || error == RoomErrors.NotFound 
           || error == UserErrors.NotFound)
            return NotFound(error);
        
        return BadRequest(error);
    }
}