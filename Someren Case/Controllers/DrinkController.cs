using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;
using System.Collections.Generic;

namespace Someren_Case.Controllers
{
    public class DrinkController : Controller
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IStudentRepository _studentRepository;

        public DrinkController(IDrinkRepository drinkRepository, IStudentRepository studentRepository)
        {
            _drinkRepository = drinkRepository;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var drinks = _drinkRepository.GetAllDrinks();
            var students = _studentRepository.GetAll();
            ViewBag.Drinks = drinks;
            ViewBag.Students = students;
            return View();
        }

        [HttpPost]
        public IActionResult Create(int studentId, int drinkId, int count)
        {
            var order = new DrinkOrder
            {
                StudentID = studentId,
                DrinkID = drinkId,
                Count = count
            };
            _drinkRepository.AddDrinkOrder(order);
            return RedirectToAction("Confirm");
        }

        public IActionResult Confirm()
        {
            return View();
        }
    }
}