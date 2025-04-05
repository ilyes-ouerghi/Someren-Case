using Someren_Case.Models;

namespace Someren_Case.Repositories
{
    public interface IStudentRepository
    {
        List<Student> GetAll(); // Fetch all students
        Student? GetById(int studentId); // Fetch a student by their ID
        void Add(Student student);
        void Update(Student student);
        void Delete(Student student);
        List<Student> Filter(string studentClass); // Optional filter method based on class (if needed)
    }
}
