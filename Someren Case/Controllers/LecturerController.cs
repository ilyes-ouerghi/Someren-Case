using Microsoft.AspNetCore.Mvc;
using Someren_Case.Interfaces;
using Someren_Case.Models;
using System.Collections.Generic;

namespace Someren_Case.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILecturerRepository _repository;

        public LecturerController(ILecturerRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            List<Lecturer> lecturers = _repository.GetAllLecturers();
            return View(lecturers);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Lecturer lecturer)
        {
            _repository.AddLecturer(lecturer);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Lecturer lecturer = _repository.GetLecturerById(id);
            return View(lecturer);
        }

        [HttpPost]
        public IActionResult Edit(Lecturer lecturer)
        {
            _repository.UpdateLecturer(lecturer);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Lecturer lecturer = _repository.GetLecturerById(id);
            return View(lecturer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            _repository.DeleteLecturer(id);
            return RedirectToAction("Index");
        }
    }
}