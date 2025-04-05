using Microsoft.AspNetCore.Mvc;
using Someren_Case.Repositories;
using Someren_Case.Models;
namespace Someren_Case.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // Display all students
        public IActionResult Index()
        {
            List<Student> students = _studentRepository.GetAll();
            return View(students);
        }

        // Search students by class (example filtering)
        [HttpPost]
        public IActionResult Filter(string studentClass)
        {
            try
            {
                List<Student> students = _studentRepository.Filter(studentClass);
                return View("Index", students);
            }
            catch (Exception)
            {
                // Optionally log the error
                return RedirectToAction("Index");
            }
        }

        // Show the form to add a new student
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Add a new student to the database
        [HttpPost]
        public IActionResult Create(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _studentRepository.Add(student);
                    return RedirectToAction("Index");
                }
                return View(student);
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return View(student);
            }
        }

        // Show the edit form for a student (GET)
        [HttpGet]
        public IActionResult Edit(int studentId)
        {
            Student student = _studentRepository.GetById(studentId);
            if (student == null)
            {
                return NotFound();  // If no student found, return 404
            }
            return View(student);  // Passing Student model to the Edit view
        }

        // Edit a student (POST)
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            try
            {
                _studentRepository.Update(student);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(student);  // Passing Student model to the Edit view if error occurs
            }

        }
        // Show the delete confirmation page (GET)
        [HttpGet]
        public IActionResult Delete(int studentId)
        {
            // Retrieve the student from the database using the studentId
            var student = _studentRepository.GetById(studentId);

            if (student == null)
            {
                return NotFound();  // Return 404 if the student is not found
            }

            return View(student);  // Pass the Student model to the Delete view
        }

        // Delete the student (POST)
        [HttpPost]
        [ActionName("Delete")]  // Explicitly name the action as Delete
        public IActionResult DeleteConfirmed(int studentId)
        {
            try
            {
                // Retrieve the student from the database using the studentId
                var student = _studentRepository.GetById(studentId);

                if (student != null)
                {
                    _studentRepository.Delete(student);  // Delete the student from the database
                }

                return RedirectToAction("Index");  // Redirect to the Index action after deletion
            }
            catch (Exception)
            {
                return RedirectToAction("Index");  // In case of an error, redirect to Index
            }
        }

    }
}
