using Microsoft.AspNetCore.Mvc;

namespace SomerenApp.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LecturerController(ApplicationDbContext context) 
        {
            _context = context;
        }

        // Action to display all lecturers
        public IActionResult Index()
        {
            var lecturers = _context.Lecturers.ToList();  // Retrieve all lecturers from the database
            return View(lecturers);  // Return the list of lecturers to the view
        }
    }
}