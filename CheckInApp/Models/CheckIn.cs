namespace CheckInApp.Models;

public class CheckIn
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset? CheckInTime { get; set; }
    public DateTimeOffset? CheckOutTime { get; set; }
}
