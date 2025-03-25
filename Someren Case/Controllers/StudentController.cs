using Microsoft.AspNetCore.Mvc;
using Someren_Case.Repositories;
using Someren_Case.Models;
using System;
using System.Collections.Generic;

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
                _studentRepository.Add(student);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(student);
            }
        }

        // Show the form to edit a student
        [HttpGet]
        public IActionResult Edit(int studentId)
        {
            Student? student = _studentRepository.GetById(studentId);
            return View(student);
        }

        // Update student details
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
                return View(student);
            }
        }

        // Show delete confirmation page
        [HttpGet]
        public IActionResult Delete(int? studentId)
        {
            if (studentId is null)
            {
                return NotFound();
            }
            else
            {
                Student? student = _studentRepository.GetById((int)studentId);
                return View(student);
            }
        }

        // Delete a student
        [HttpPost]
        public IActionResult Delete(Student student)
        {
            try
            {
                _studentRepository.Delete(student);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(student);
            }
        }
    }
}
