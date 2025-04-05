using Someren_Case.Interfaces;
using Someren_Case.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Someren_Case.Repositories
{
    public class DbDrinkRepository : IDrinkRepository
    {
        private readonly string _connectionString;

        public DbDrinkRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Drink> GetAllDrinks()
        {
            var drinks = new List<Drink>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DrinkID, Name, Type, VATRate, StockQuantity FROM Drink", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    drinks.Add(new Drink
                    {
                        DrinkID = (int)reader["DrinkID"],
                        Name = reader["Name"].ToString(),
                        Type = reader["Type"].ToString(),
                        VATRate = (decimal)reader["VATRate"],
                        StockQuantity = (int)reader["StockQuantity"]
                    });
                }
            }
            return drinks;
        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT StudentID, StudentNumber, FirstName, LastName, PhoneNumber, Class FROM Student", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentID = (int)reader["StudentID"],
                        StudentNumber = reader["StudentNumber"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Class = reader["Class"].ToString()
                    });
                }
            }
            return students;
        }

        public void AddDrinkOrder(DrinkOrder order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO DrinkOrder (StudentID, DrinkID, Quantity) VALUES (@StudentID, @DrinkID, @Quantity)", connection);
                command.Parameters.AddWithValue("@StudentID", order.StudentID);
                command.Parameters.AddWithValue("@DrinkID", order.DrinkID);
                command.Parameters.AddWithValue("@Quantity", order.Quantity);

                command.ExecuteNonQuery();
            }
        }
    }
}
