using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;
using System.Collections.Generic;

namespace Someren.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IActionResult Index()
        {
            List<Room> rooms = _roomRepository.GetAllRooms();
            return View(rooms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                _roomRepository.AddRoom(room);
                return RedirectToAction("Index");
            }
            return View(room);
        }

        public IActionResult Edit(int id)
        {
            Room room = _roomRepository.GetRoomById(id);
            if (room == null)
                return NotFound();

            return View(room);
        }

        [HttpPost]
        public IActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                _roomRepository.UpdateRoom(room);
                return RedirectToAction("Index");
            }
            return View(room);
        }

        public IActionResult Delete(int id)
        {
            Room room = _roomRepository.GetRoomById(id);
            if (room == null)
                return NotFound();

            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _roomRepository.DeleteRoom(id);
            return RedirectToAction("Index");
        }
    }
}
