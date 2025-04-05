using System;
using Microsoft.Data.SqlClient; 
using Someren_Case.Interfaces;

namespace Someren_Case.Repositories
{
    public class DbDrinkOrderRepository : IDrinkOrderRepository
    {
        private readonly string _connectionString;

        public DbDrinkOrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddDrinkOrder(int studentId, int drinkId, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO DrinkOrders (StudentID, DrinkID, Quantity, OrderDate) 
                                 VALUES (@studentId, @drinkId, @quantity, @orderDate)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentId", studentId);
                cmd.Parameters.AddWithValue("@drinkId", drinkId);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@orderDate", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
