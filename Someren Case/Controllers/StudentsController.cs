using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Someren_Case.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Someren_Case.Controllers
{
    public class StudentsController : Controller
    {
        private readonly string _connectionString;

        public StudentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Student";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = (int)reader["StudentId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        StudentNumber = reader["StudentNumber"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Class = reader["Class"].ToString()
                    });
                }
            }

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query =
                        "INSERT INTO Student (FirstName, LastName, StudentNumber, PhoneNumber, Class) VALUES (@FirstName, @LastName, @StudentNumber, @PhoneNumber, @Class)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);
                    command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    command.Parameters.AddWithValue("@Class", student.Class);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        // GET: Students/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            Student student = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Student WHERE StudentId = @StudentId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", id);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    student = new Student
                    {
                        StudentId = (int)reader["StudentId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        StudentNumber = reader["StudentNumber"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Class = reader["Class"].ToString()
                    };
                }
            }

            if (student == null) return NotFound();
            return View(student);
        }

        // POST: Students/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentId) return NotFound();
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query =
                        "UPDATE Student SET FirstName = @FirstName, LastName = @LastName, StudentNumber = @StudentNumber, PhoneNumber = @PhoneNumber, Class = @Class WHERE StudentId = @StudentId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);
                    command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    command.Parameters.AddWithValue("@Class", student.Class);
                    command.Parameters.AddWithValue("@StudentId", student.StudentId);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        // GET: Students/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            Student student = null;

            // Establishing the connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Student WHERE StudentId = @StudentId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", id); // Setting the parameter for StudentId
                connection.Open();

                // Executing the query and reading the data
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    student = new Student
                    {
                        StudentId = (int)reader["StudentId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        StudentNumber = reader["StudentNumber"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Class = reader["Class"].ToString()
                    };
                }
            }

            // If no student is found, return NotFound.
            if (student == null)
            {
                return NotFound();
            }

            // Return the student data for deletion confirmation
            return View(student);
        }

    

    // POST: Students/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Student WHERE StudentId = @StudentId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", id);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}