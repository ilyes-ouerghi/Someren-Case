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
    public class ActivityController : Controller
    {
        private readonly string _connectionString;

        public ActivityController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Activity/Index
        public async Task<IActionResult> Index()
        {
            List<Activity> activities = new List<Activity>();

            // Get all activities
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Activity";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    activities.Add(new Activity
                    {
                        ActivityID = (int)reader["ActivityID"],
                        ActivityName = reader["ActivityName"].ToString(),
                        Date = (DateTime)reader["Date"],
                        StartTime = (TimeSpan)reader["StartTime"],
                        EndTime = (TimeSpan)reader["EndTime"]
                    });
                }
            }

            // If no activities are found, show error message
            if (activities.Count == 0)
            {
                ViewData["ErrorMessage"] = "No activities found.";
                return View();
            }

            return View(activities);
        }

        // GET: Activity/Manage/{activityId}
        public async Task<IActionResult> Manage(int activityId)
        {
            Activity activity = null;
            List<Student> participatingStudents = new List<Student>();
            List<Student> nonParticipatingStudents = new List<Student>();

            // Fetch activity details
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

            // Check if activity is found
            if (activity == null)
            {
                ViewData["ErrorMessage"] = "No activity found with the provided ID.";
                return View();
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Student";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                List<Student> allStudents = new List<Student>();
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

                nonParticipatingStudents = allStudents.Except(participatingStudents).ToList();
            }

            var model = new ActivityManagementViewModel
            {
                Activity = activity,
                ParticipatingStudents = participatingStudents,
                NonParticipatingStudents = nonParticipatingStudents
            };

            return View(model);
        }

        // GET: Activity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activity/Create
        [HttpPost]
        public async Task<IActionResult> Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query =
                        "INSERT INTO Activity (ActivityName, Date, StartTime, EndTime) VALUES (@ActivityName, @Date, @StartTime, @EndTime)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                    command.Parameters.AddWithValue("@Date", activity.Date);
                    command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                    command.Parameters.AddWithValue("@EndTime", activity.EndTime);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(activity);
        }

        // GET: Activity/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            Activity activity = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Activity WHERE ActivityID = @ActivityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", id);
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

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activity/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Activity activity)
        {
            if (id != activity.ActivityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query =
                        "UPDATE Activity SET ActivityName = @ActivityName, Date = @Date, StartTime = @StartTime, EndTime = @EndTime WHERE ActivityID = @ActivityID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                    command.Parameters.AddWithValue("@Date", activity.Date);
                    command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                    command.Parameters.AddWithValue("@EndTime", activity.EndTime);
                    command.Parameters.AddWithValue("@ActivityID", activity.ActivityID);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(activity);
        }

        // GET: Activity/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            Activity activity = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Activity WHERE ActivityID = @ActivityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", id);
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

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activity/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Activity WHERE ActivityID = @ActivityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", id);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Activity/AddParticipant/{activityId}/{studentId}
        [HttpPost]
        public async Task<IActionResult> AddParticipant(int activityId, int studentId)
        {
            // Check if the student is already participating in the activity
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM Participate WHERE ActivityID = @ActivityID AND StudentID = @StudentID";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@ActivityID", activityId);
                checkCommand.Parameters.AddWithValue("@StudentID", studentId);
                connection.Open();

                int count = (int)await checkCommand.ExecuteScalarAsync();

                // If the student is already participating, do not insert again
                if (count > 0)
                {
                    // You can add a message or return a different result if needed.
                    ViewData["ErrorMessage"] = "The student is already participating in this activity.";
                    return RedirectToAction(nameof(Manage), new { activityId });
                }
            }

            // If the student is not already participating, insert the participation
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Participate (ActivityID, StudentID) VALUES (@ActivityID, @StudentID)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityID", activityId);
                command.Parameters.AddWithValue("@StudentID", studentId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            // Now, refresh the NonParticipatingStudents list for the Manage view
            return RedirectToAction(nameof(Manage), new { activityId });
        }


        // POST: Activity/RemoveParticipant/{activityId}/{studentId}
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

            return RedirectToAction(nameof(Manage), new { activityId });
        }
    }
}
