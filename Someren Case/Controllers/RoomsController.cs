using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Someren_Case.Data;
using Someren_Case.Models;
using System;
using System.Linq;

namespace Someren_Case.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context; // ? Fix: Use AppDbContext

        public RoomsController(ApplicationDbContext context) // ? Fix: Use correct DbContext
        {
            _context = context;
        }

        // ? 1. Display all rooms
        public IActionResult Index()
        {
            var rooms = _context.Room.ToList(); // Fetch all rooms from the database
            return View(rooms);
        }

        // ? 2. Show create form
        public IActionResult Create()
        {
            return View();
        }

        // ? 3. Handle new room creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Room.Add(room);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // ? 4. Show edit form
        public IActionResult Edit(int id)
        {
            var room = _context.Room.Find(id);
            if (room == null) return NotFound();
            return View(room);
        }

        // ? 5. Handle updating a room
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Room room)
        {
            if (id != room.RoomID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Room.Update(room);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // ? 6. Show delete confirmation
        public IActionResult Delete(int id)
        {
            var room = _context.Room.Find(id);
            if (room == null) return NotFound();
            return View(room);
        }

        // ? 7. Handle deletion of a room
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var room = _context.Room.Find(id);
            if (room != null)
            {
                _context.Room.Remove(room);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
