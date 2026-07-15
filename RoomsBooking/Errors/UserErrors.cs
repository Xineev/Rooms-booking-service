using RoomsBooking.Common;

namespace RoomsBooking.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =  new Error("User.NotFound", "User was not found");
}