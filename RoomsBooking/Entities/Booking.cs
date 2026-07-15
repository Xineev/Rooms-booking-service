namespace RoomsBooking.Entities;

public class Booking
{
    public int Id { get; set; }
    
    public int UserId { get; set; }

    public User User { get; set; } = null!;
    
    public int RoomId { get; set; }

    public Room Room { get; set; } = null!;
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public BookingStatus Status { get; set; }

    public void UpdateTime(DateTime newStartTime, DateTime newEndTime)
    {
        StartTime = newStartTime;
        EndTime = newEndTime;
        UpdatedAt = DateTime.Now;
    }
}

public enum BookingStatus
{
    Active,
    Canceled,
    Completed
}