using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Someren_Case.Controllers
{
    public class ActivityParticipantsController : Controller
    {
        private readonly string _connectionString;

        public ActivityParticipantsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: ActivityParticipants/Index/{activityId}
        public async Task<IActionResult> Index(int activityId)
        {
            List<Student> allStudents = new List<Student>();
            List<Student> participatingStudents = new List<Student>();
            List<Student> nonParticipatingStudents = new List<Student>();

            // Get all students
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Student";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    allStudents.Add(new Student
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

            // Get students who are participating in the activity
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT s.* FROM Student s " +
                               "JOIN Participate p ON s.StudentId = p.StudentID " +
                               "WHERE p.ActivityID = @ActivityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", activityId);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    participatingStudents.Add(new Student
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

            // Get students who are NOT participating in the activity
            nonParticipatingStudents = allStudents.Except(participatingStudents).ToList();

            // Fetch activity details
            Activity activity = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Activity WHERE ActivityID = @ActivityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", activityId);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    activity = new Activity
                    {
                        ActivityID = (int)reader["ActivityID"],
                        ActivityName = reader["ActivityName"].ToString(),
                        Date = (DateTime)reader["Date"],
                        StartTime = (TimeSpan)reader["StartTime"],
                        EndTime = (TimeSpan)reader["EndTime"]
                    };
                }
            }

            var model = new ActivityManagementViewModel
            {
                Activity = activity,
                ParticipatingStudents = participatingStudents,
                NonParticipatingStudents = nonParticipatingStudents
            };

            return View(model);
        }

        // POST: ActivityParticipants/AddParticipant/{activityId}/{studentId}
        [HttpPost]
        public async Task<IActionResult> AddParticipant(int activityId, int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Participate (ActivityID, StudentID) VALUES (@ActivityID, @StudentID)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", activityId);
                command.Parameters.AddWithValue("@StudentID", studentId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index), new { activityId });
        }

        // POST: ActivityParticipants/RemoveParticipant/{activityId}/{studentId}
        [HttpPost]
        public async Task<IActionResult> RemoveParticipant(int activityId, int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Participate WHERE ActivityID = @ActivityID AND StudentID = @StudentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", activityId);
                command.Parameters.AddWithValue("@StudentID", studentId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index), new { activityId });
        }
    }
}
