using Someren_Case.Models;
using System.Collections.Generic;

namespace Someren_Case.Repositories
{
    public interface ILecturerRepository
    {
        List<Lecturer> GetAllLecturers();
        void AddLecturer(Lecturer lecturer);
        void UpdateLecturer(Lecturer lecturer);
        void DeleteLecturer(int id);
        Lecturer GetLecturerById(int id);
    }
}
