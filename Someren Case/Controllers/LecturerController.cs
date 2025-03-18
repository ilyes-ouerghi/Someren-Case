using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Someren_Case.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace Someren_Case.Controllers
{
    public class LecturersController : Controller
    {
        private readonly string _connectionString;

        public LecturersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Lecturers (Read)
        public async Task<IActionResult> Index()
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Lecturer";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        lecturers.Add(new Lecturer
                        {
                            LecturerID = (int)reader["LecturerID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            DateOfBirth = (DateTime)reader["DateOfBirth"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Error loading data: " + ex.Message;
            }

            return View(lecturers);
        }

        // GET: Lecturers/Create (Show form)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecturers/Create (Insert into database)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        string query =
                            "INSERT INTO Lecturer (FirstName, LastName, PhoneNumber, DateOfBirth) VALUES (@FirstName, @LastName, @PhoneNumber, @DateOfBirth)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                        command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                        command.Parameters.AddWithValue("@DateOfBirth", lecturer.DateOfBirth);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error inserting data: " + ex.Message);
                }
            }

            return View(lecturer);
        }

        // GET: Lecturers/Edit/{id} (Show edit form)
        public async Task<IActionResult> Edit(int id)
        {
            Lecturer lecturer = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Lecturer WHERE LecturerID = @LecturerID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@LecturerID", id);
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        lecturer = new Lecturer
                        {
                            LecturerID = (int)reader["LecturerID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            DateOfBirth = (DateTime)reader["DateOfBirth"]
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Error loading data: " + ex.Message;
            }

            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // POST: Lecturers/Edit/{id} (Update in database)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lecturer lecturer)
        {
            if (id != lecturer.LecturerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        string query =
                            "UPDATE Lecturer SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, DateOfBirth = @DateOfBirth WHERE LecturerID = @LecturerID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@LecturerID", lecturer.LecturerID);
                        command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                        command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                        command.Parameters.AddWithValue("@DateOfBirth", lecturer.DateOfBirth);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating data: " + ex.Message);
                }
            }

            return View(lecturer);
        }

        // GET: Lecturers/Delete/{id} (Show delete confirmation page)
        public async Task<IActionResult> Delete(int id)
        {
            Lecturer lecturer = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Lecturer WHERE LecturerID = @LecturerID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@LecturerID", id);
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        lecturer = new Lecturer
                        {
                            LecturerID = (int)reader["LecturerID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            DateOfBirth = (DateTime)reader["DateOfBirth"]
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Error loading data: " + ex.Message;
            }

            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // POST: Lecturers/Delete/{id} (Delete from database)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Lecturer WHERE LecturerID = @LecturerID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@LecturerID", id);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Error deleting data: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}