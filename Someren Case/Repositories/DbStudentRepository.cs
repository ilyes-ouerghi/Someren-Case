using Someren_Case.Models;
using System.Data.SqlClient;

namespace Someren_Case.Repositories
{
    public class DbStudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public DbStudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

       
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

       
        public List<Student> GetStudentsByRoomId(int roomId)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT s.StudentID, s.StudentNumber, s.FirstName, s.LastName, s.PhoneNumber, s.Class " +
                               "FROM Student s " +
                               "INNER JOIN Share si ON s.StudentID = si.StudentID " +
                               "WHERE si.RoomID = @RoomID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", roomId);

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

        
        public List<Student> GetStudentsWithoutRoom()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT StudentID, StudentNumber, FirstName, LastName, PhoneNumber, Class " +
                               "FROM Student " +
                               "WHERE StudentID NOT IN (SELECT StudentID FROM Share)";

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

       
        public void AssignStudentToRoom(int studentId, int roomId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Share (StudentID, RoomID) VALUES (@StudentID, @RoomID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@RoomID", roomId);

                    command.ExecuteNonQuery();
                }
            }
        }

      
        public void RemoveStudentFromRoom(int studentId, int roomId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Share WHERE StudentID = @StudentID AND RoomID = @RoomID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@RoomID", roomId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
