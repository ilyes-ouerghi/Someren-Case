using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Someren_Case.Models;

namespace Someren_Case.Repositories
{
    public class DbLecturerRepository : ILecturerRepository
    {
        private readonly string _connectionString;

        // Constructor to inject IConfiguration and retrieve the connection string
        public DbLecturerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Lecturer> GetAllLecturers()
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Lecturers", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lecturers.Add(new Lecturer
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Age = (int)reader["Age"]
                    });
                }
            }
            return lecturers;
        }

        public void AddLecturer(Lecturer lecturer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Lecturers (FirstName, LastName, PhoneNumber, Age) VALUES (@FirstName, @LastName, @PhoneNumber, @Age)", connection);
                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                command.Parameters.AddWithValue("@Age", lecturer.Age);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Lecturers SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Age = @Age WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", lecturer.Id);
                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                command.Parameters.AddWithValue("@Age", lecturer.Age);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteLecturer(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Lecturers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public Lecturer GetLecturerById(int id)
        {
            Lecturer lecturer = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Lecturers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    lecturer = new Lecturer
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Age = (int)reader["Age"]
                    };
                }
            }
            return lecturer;
        }
    }
}
