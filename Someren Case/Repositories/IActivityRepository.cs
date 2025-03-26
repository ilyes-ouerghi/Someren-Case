using Someren_Case.Models;

public interface IActivityRepository
{
    List<Activity> GetAll(); // Get all activities
    Activity GetById(int activityId); // Get activity by ID
    void Add(Activity activity); // Add new activity
    void Update(Activity activity); // Update existing activity
    void Delete(Activity activity); // Delete an activity
    
}