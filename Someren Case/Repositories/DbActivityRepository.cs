using System.Data.SqlClient;
using Someren_Case.Models;

namespace Someren_Case.Repositories
{
    public class DbActivityRepository : IActivityRepository
    {
        private readonly string _connectionString;

        public DbActivityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get all activities
        public List<Activity> GetAll()
        {
            List<Activity> activities = new List<Activity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ActivityID, ActivityName, Date, StartTime, EndTime FROM Activity";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        activities.Add(new Activity
                        {
                            ActivityID = reader.GetInt32(0),
                            ActivityName = reader.GetString(1),
                            Date = reader.GetDateTime(2),
                            StartTime = reader.GetTimeSpan(3),
                            EndTime = reader.GetTimeSpan(4)
                        });
                    }
                }
            }

            return activities;
        }

        // Get activity by ID
        public Activity GetById(int activityId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ActivityID, ActivityName, Date, StartTime, EndTime FROM Activity WHERE ActivityID = @ActivityID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activityId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Activity
                            {
                                ActivityID = reader.GetInt32(0),
                                ActivityName = reader.GetString(1),
                                Date = reader.GetDateTime(2),
                                StartTime = reader.GetTimeSpan(3),
                                EndTime = reader.GetTimeSpan(4)
                            };
                        }
                    }
                }
            }

            return null;
        }

        // Add a new activity
        public void Add(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Activity (ActivityName, Date, StartTime, EndTime) VALUES (@ActivityName, @Date, @StartTime, @EndTime)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                    command.Parameters.AddWithValue("@Date", activity.Date);
                    command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                    command.Parameters.AddWithValue("@EndTime", activity.EndTime);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Update activity
        public void Update(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Activity SET ActivityName = @ActivityName, Date = @Date, StartTime = @StartTime, EndTime = @EndTime WHERE ActivityID = @ActivityID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activity.ActivityID);
                    command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                    command.Parameters.AddWithValue("@Date", activity.Date);
                    command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                    command.Parameters.AddWithValue("@EndTime", activity.EndTime);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete activity
        public void Delete(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Activity WHERE ActivityID = @ActivityID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activity.ActivityID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Add participant to activity
        public void AddParticipantToActivity(int studentId, int activityId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO ActivityParticipant (StudentID, ActivityID) VALUES (@StudentID, @ActivityID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@ActivityID", activityId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Remove participant from activity
        public void RemoveParticipantFromActivity(int studentId, int activityId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM ActivityParticipant WHERE StudentID = @StudentID AND ActivityID = @ActivityID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@ActivityID", activityId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Get list of students in an activity
        public List<Student> GetStudentsInActivity(int activityId)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT s.StudentID, s.StudentNumber, s.FirstName, s.LastName, s.PhoneNumber, s.Class FROM Student s " +
                               "JOIN ActivityParticipant ap ON s.StudentID = ap.StudentID " +
                               "WHERE ap.ActivityID = @ActivityID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activityId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                StudentID = reader.GetInt32(0),
                                StudentNumber = reader.GetString(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Class = reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return students;
        }
    }
}
