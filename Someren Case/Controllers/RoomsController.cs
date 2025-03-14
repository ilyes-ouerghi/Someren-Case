using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Someren_Case.Data;
using System.Linq;

namespace Someren_Case.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rooms = _context.Room.ToList(); // Fetch all rooms from the database
            return View(rooms);
        }
    }
}