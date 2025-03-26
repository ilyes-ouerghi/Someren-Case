namespace Someren_Case.Models;

public class Activity
{
    public int ActivityID { get; set; }
    public string ActivityName { get; set; }
    public DateTime Date { get; set; }  // Date of the activity
    public TimeSpan StartTime { get; set; }  // Start time of the activity
    public TimeSpan EndTime { get; set; }  // End time of the activity
}
