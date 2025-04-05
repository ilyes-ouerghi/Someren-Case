using Someren_Case.Models;
using Microsoft.Data.SqlClient;

namespace Someren_Case.Repositories
{
    public class DbStudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public DbStudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get all students
        public List<Student> GetAll()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT StudentID, StudentNumber, FirstName, LastName, PhoneNumber, Class FROM Student";

                using (SqlCommand command = new SqlCommand(query, connection))
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

            return students;
        }

        // Get a student by ID
        public Student? GetById(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT StudentID, StudentNumber, FirstName, LastName, PhoneNumber, Class FROM Student WHERE StudentID = @StudentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student
                            {
                                StudentID = reader.GetInt32(0),
                                StudentNumber = reader.GetString(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Class = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        // Add a new student
        public void Add(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Student (StudentNumber, FirstName, LastName, PhoneNumber, Class) VALUES (@StudentNumber, @FirstName, @LastName, @PhoneNumber, @Class)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    command.Parameters.AddWithValue("@Class", student.Class);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Update student details
        public void Update(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Student SET StudentNumber=@StudentNumber, FirstName=@FirstName, LastName=@LastName, PhoneNumber=@PhoneNumber, Class=@Class WHERE StudentID=@StudentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    command.Parameters.AddWithValue("@Class", student.Class);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete a student
        public void Delete(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Student WHERE StudentID=@StudentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Filter students by class
        public List<Student> Filter(string studentClass)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT StudentID, StudentNumber, FirstName, LastName, PhoneNumber, Class FROM Student WHERE Class = @Class";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Class", studentClass);

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

        // Get students participating in a specific activity
        public List<Student> GetStudentsInActivity(int activityId)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT s.StudentID, s.StudentNumber, s.FirstName, s.LastName, s.PhoneNumber, s.Class " +
                               "FROM Student s " +
                               "INNER JOIN ActivityParticipants ap ON s.StudentID = ap.StudentID " +
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

        // Get students not participating in a specific activity
        public List<Student> GetStudentsNotInActivity(int activityId)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT s.StudentID, s.StudentNumber, s.FirstName, s.LastName, s.PhoneNumber, s.Class " +
                               "FROM Student s " +
                               "WHERE s.StudentID NOT IN (SELECT StudentID FROM ActivityParticipants WHERE ActivityID = @ActivityID)";

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

        // Add a student to a specific activity
        public void AddParticipant(int activityId, int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO ActivityParticipants (ActivityID, StudentID) VALUES (@ActivityID, @StudentID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activityId);
                    command.Parameters.AddWithValue("@StudentID", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Remove a student from a specific activity
        public void RemoveParticipant(int activityId, int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM ActivityParticipants WHERE ActivityID = @ActivityID AND StudentID = @StudentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activityId);
                    command.Parameters.AddWithValue("@StudentID", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}