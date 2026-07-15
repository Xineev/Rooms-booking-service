using RoomsBooking.Common;

namespace RoomsBooking.Errors;

public static class BookingErrors
{
    public static readonly Error NotFound = new Error("Booking.NotFound", "Booking was not found");
    
    public static readonly Error BookingsOverlap = new Error("Booking.BookingsOverlap", 
        "New booking overlap with one existing");
    
    public static readonly Error InvalidEndTime = new Error("Booking.InvalidTime", 
        "End time is earlier than start time");
    
    public static readonly Error BookingInPast = new Error("Booking.BookingInPast", 
        "Start time is earlier than current time");
    
    public static readonly Error BookingAlreadyCompleted  = new Error("Booking.BookingAlreadyCompleted",
        "Booking already completed");
}