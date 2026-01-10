namespace CheckInApp.Models;

public class CheckIn
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
}
