namespace Someren_Case.Models
{
    public class Lecturer
    {
        public int LecturerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; } // Changed from int to DateTime
    }
}

