using Someren_Case.Models;
using System.Collections.Generic;
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
                var command = new SqlCommand("SELECT * FROM Drink", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drinks.Add(new Drink
                        {
                            DrinkID = (int)reader["DrinkID"],
                            Name = (string)reader["Name"],
                            Type = (string)reader["Type"],
                            VATRate = (decimal)reader["VATRate"],
                            StockQuantity = (int)reader["StockQuantity"]
                        });
                    }
                }
            }

            return drinks;
        }

        public void AddDrinkOrder(DrinkOrder order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO OrderTable (StudentID, DrinkID, Quantity) VALUES (@StudentID, @DrinkID, @Quantity)", connection);
                command.Parameters.AddWithValue("@StudentID", order.StudentID);
                command.Parameters.AddWithValue("@DrinkID", order.DrinkID);
                command.Parameters.AddWithValue("@Quantity", order.Count);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}