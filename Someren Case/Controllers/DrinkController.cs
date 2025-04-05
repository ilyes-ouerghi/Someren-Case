using Microsoft.AspNetCore.Mvc;
using Someren_Case.Interfaces;
using Someren_Case.Models;

namespace Someren_Case.Controllers
{
    public class DrinkController : Controller
    {
        private readonly IDrinkRepository _drinkRepository;

        public DrinkController(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        // GET: Drink/ProcessOrder
        public IActionResult ProcessOrder()
        {
            // Get all drinks and students from the repository
            var drinks = _drinkRepository.GetAllDrinks();
            var students = _drinkRepository.GetAllStudents();

            // Pass them to the view via ViewBag
            ViewBag.Drinks = drinks;
            ViewBag.Students = students;

            return View();
        }

        // POST: Drink/ProcessOrder
        [HttpPost]
        public IActionResult ProcessOrder(int selectedStudentID, int selectedDrinkID, int quantity)
        {
            // Process the order
            var order = new DrinkOrder
            {
                StudentID = selectedStudentID,
                DrinkID = selectedDrinkID,
                Quantity = quantity
            };
            _drinkRepository.AddDrinkOrder(order);

            return RedirectToAction("OrderConfirmation");
        }

        // GET: Drink/OrderConfirmation
        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
