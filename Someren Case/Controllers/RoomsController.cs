using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Someren_Case.Data;
using Someren_Case.Models;
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
            var rooms = _context.Room.ToList();
            return View(rooms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Room (FloorNumber, NumberOfBeds, Building, RoomType) " +
                               "VALUES (@FloorNumber, @NumberOfBeds, @Building, @RoomType);";

                using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FloorNumber", room.FloorNumber);
                    command.Parameters.AddWithValue("@NumberOfBeds", room.NumberOfBeds);
                    command.Parameters.AddWithValue("@Building", room.Building);
                    command.Parameters.AddWithValue("@RoomType", room.RoomType);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        public IActionResult Edit(int id)
        {
            var room = _context.Room.Find(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Room room)
        {
            if (id != room.RoomID) return NotFound();

            if (ModelState.IsValid)
            {
                string query = "UPDATE Room SET FloorNumber = @FloorNumber, NumberOfBeds = @NumberOfBeds, " +
                               "Building = @Building, RoomType = @RoomType WHERE RoomID = @RoomID;";

                using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RoomID", room.RoomID);
                    command.Parameters.AddWithValue("@FloorNumber", room.FloorNumber);
                    command.Parameters.AddWithValue("@NumberOfBeds", room.NumberOfBeds);
                    command.Parameters.AddWithValue("@Building", room.Building);
                    command.Parameters.AddWithValue("@RoomType", room.RoomType);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        public IActionResult Delete(int id)
        {
            var room = _context.Room.Find(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            string query = "DELETE FROM Room WHERE RoomID = @RoomID;";

            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}