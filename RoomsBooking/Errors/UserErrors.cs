using RoomsBooking.Common;

namespace RoomsBooking.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =  new Error("User.NotFound", "User was not found");
    
    public static readonly Error AlreadyExists = new Error("User.AlreadyExists", "User already exists");
}