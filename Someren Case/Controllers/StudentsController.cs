using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Someren_Case.Data;
using Someren_Case.Models;

namespace Someren_Case.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Student.ToListAsync();
            return View(students);
        }
    }
}