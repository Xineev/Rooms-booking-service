using RoomsBooking.Common;

namespace RoomsBooking.Errors;

public static class RoomErrors
{
    public static readonly Error NotFound = new Error("Room.NotFound",  "Room was not found");
    
    public static readonly Error AlreadyExists = new Error("Room.AlreadyExists", "Room already exists");
}