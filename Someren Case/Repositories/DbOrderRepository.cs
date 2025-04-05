using Microsoft.Data.SqlClient;
using Someren_Case.Models;
using System.Collections.Generic;

public class DbOrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public DbOrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

   
    public void AddOrder(Order order)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO [OrderTable] (StudentID, DrinkID, Quantity) VALUES (@studentId, @drinkId, @quantity)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@studentId", order.StudentID);
            cmd.Parameters.AddWithValue("@drinkId", order.DrinkID);
            cmd.Parameters.AddWithValue("@quantity", order.Quantity);

            conn.Open();
            cmd.ExecuteNonQuery(); 
        }
    }
    
    public List<Order> GetAllOrders()
    {
        List<Order> orders = new List<Order>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "SELECT OrderID, StudentID, DrinkID, Quantity FROM [OrderTable]"; 
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Order order = new Order
                {
                    OrderID = reader.GetInt32(0),  
                    StudentID = reader.GetInt32(1), 
                    DrinkID = reader.GetInt32(2),  
                    Quantity = reader.GetInt32(3)  
                };

                orders.Add(order);
            }
        }

        return orders;
    }
}