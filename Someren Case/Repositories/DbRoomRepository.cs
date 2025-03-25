using Someren.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Someren.Repositories
{
    public class DbRoomRepository : IRoomRepository
    {
        private readonly string _connectionString;

        public DbRoomRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbproject242504")
                ?? throw new ArgumentNullException("Connection string not found!");
        }

        public List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT RoomID, FloorNumber, NumberOfBeds, Building, RoomType FROM Room";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rooms.Add(new Room
                    {
                        RoomID = Convert.ToInt32(reader["RoomID"]),
                        FloorNumber = reader["FloorNumber"] as int?,
                        NumberOfBeds = reader["NumberOfBeds"] as int?,
                        Building = reader["Building"].ToString(),
                        RoomType = reader["RoomType"].ToString()
                    });
                }
            }
            return rooms;
        }

        public Room GetRoomById(int id)
        {
            Room room = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT RoomID, FloorNumber, NumberOfBeds, Building, RoomType FROM Room WHERE RoomID = @RoomID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    room = new Room
                    {
                        RoomID = Convert.ToInt32(reader["RoomID"]),
                        FloorNumber = reader["FloorNumber"] as int?,
                        NumberOfBeds = reader["NumberOfBeds"] as int?,
                        Building = reader["Building"].ToString(),
                        RoomType = reader["RoomType"].ToString()
                    };
                }
            }
            return room;
        }

        public void AddRoom(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Room (FloorNumber, NumberOfBeds, Building, RoomType) VALUES (@FloorNumber, @NumberOfBeds, @Building, @RoomType)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FloorNumber", room.FloorNumber);
                command.Parameters.AddWithValue("@NumberOfBeds", room.NumberOfBeds);
                command.Parameters.AddWithValue("@Building", room.Building);
                command.Parameters.AddWithValue("@RoomType", room.RoomType);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateRoom(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Room SET FloorNumber = @FloorNumber, NumberOfBeds = @NumberOfBeds, Building = @Building, RoomType = @RoomType WHERE RoomID = @RoomID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", room.RoomID);
                command.Parameters.AddWithValue("@FloorNumber", room.FloorNumber);
                command.Parameters.AddWithValue("@NumberOfBeds", room.NumberOfBeds);
                command.Parameters.AddWithValue("@Building", room.Building);
                command.Parameters.AddWithValue("@RoomType", room.RoomType);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteRoom(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Room WHERE RoomID = @RoomID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
