using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Someren_Case.Interfaces;
using Someren_Case.Models;

namespace Someren_Case.Repositories
{
    public class DbLecturerRepository : ILecturerRepository
    {
        private readonly string _connectionString;

        public DbLecturerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Lecturer> GetAllLecturers()
        {
            var lecturers = new List<Lecturer>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT LecturerID, FirstName, LastName, PhoneNumber, DateOfBirth FROM Lecturer";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lecturers.Add(new Lecturer
                        {
                            LecturerID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            DateOfBirth = reader.GetDateTime(4) // ✅ Fixed Type
                        });
                    }
                }
            }
            return lecturers;
        }

        public Lecturer GetLecturerById(int id)
        {
            Lecturer lecturer = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT LecturerID, FirstName, LastName, PhoneNumber, DateOfBirth FROM Lecturer WHERE LecturerID = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lecturer = new Lecturer
                            {
                                LecturerID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                PhoneNumber = reader.GetString(3),
                                DateOfBirth = reader.GetDateTime(4) // ✅ Fixed Type
                            };
                        }
                    }
                }
            }
            return lecturer;
        }

        public void AddLecturer(Lecturer lecturer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Lecturer (FirstName, LastName, PhoneNumber, DateOfBirth) VALUES (@FirstName, @LastName, @PhoneNumber, @DateOfBirth)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", lecturer.LastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@DateOfBirth", lecturer.DateOfBirth); // ✅ Ensures DateTime type
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE Lecturer SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, DateOfBirth = @DateOfBirth WHERE LecturerID = @LecturerID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", lecturer.LastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@DateOfBirth", lecturer.DateOfBirth); // ✅ Ensures DateTime type
                    cmd.Parameters.AddWithValue("@LecturerID", lecturer.LecturerID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteLecturer(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Lecturer WHERE LecturerID = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
