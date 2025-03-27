using Someren_Case.Models;
using System.Collections.Generic;

namespace Someren_Case.Repositories
{
    public interface IStudentRepository
    {
        List<Student> GetAll();  
        Student? GetById(int studentId);
        void Add(Student student);
        void Update(Student student);
        void Delete(Student student);
        List<Student> Filter(string studentClass);
        
        // New methods for handling students and rooms
        List<Student> GetStudentsByRoomId(int roomId); // Fetch students by room ID
        void AssignStudentToRoom(int studentId, int roomId); // Assign a student to a room
        void RemoveStudentFromRoom(int studentId, int roomId); // Remove a student from a room

        // New method to get students without a room
        List<Student> GetStudentsWithoutRoom();  // This method should go here
    }
}