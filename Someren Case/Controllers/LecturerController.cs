using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Someren_Case.Models;
using Microsoft.Extensions.Configuration;

namespace Someren_Case.Controllers
{
    public class LecturersController : Controller
    {
        private readonly string _connectionString;

        public LecturersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Lecturers
        public async Task<IActionResult> Index()
        {
            List<Lecturer> lecturers = new List<Lecturer>();
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
                        DateOfBirth = (DateTime)reader["DateOfBirth"],

                    });
                }
            }

            return View(lecturers);
        }
    }
}