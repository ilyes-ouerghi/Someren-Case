namespace Someren_Case.Models
{
    public class ActivityManagementViewModel
    {
        public Activity Activity { get; set; }
        public List<Student> ParticipatingStudents { get; set; }
        public List<Student> NonParticipatingStudents { get; set; }
    }
}