using Microsoft.EntityFrameworkCore;
using RoomsBooking.Entities;

namespace RoomsBooking.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    
    public DbSet<Room> Rooms => Set<Room>();
    
    public DbSet<Booking> Bookings =>  Set<Booking>();
    
}