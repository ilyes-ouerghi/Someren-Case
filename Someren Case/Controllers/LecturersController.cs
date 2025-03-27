using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;

namespace Someren_Case.Controllers
{
    public class LecturersController : Controller
    {
        private readonly ILecturerRepository _lecturerRepository;

        public LecturersController(ILecturerRepository lecturerRepository)
        {
            _lecturerRepository = lecturerRepository;
        }

        // Index Action
        public IActionResult Index()
        {
            var lecturers = _lecturerRepository.GetAllLecturers(); // Assuming you have a method to fetch lecturers
            return View(lecturers);
        }

        // Create Action (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Create Action (POST)
        [HttpPost]
        public IActionResult Create(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _lecturerRepository.AddLecturer(lecturer);
                return RedirectToAction("Index");
            }

            return View(lecturer);
        }

        // Add other CRUD actions (Edit, Delete, etc.)
    }
}
