namespace Someren_Case.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public int FloorNumber { get; set; } // Make sure this is an int
        public int NumberOfBeds { get; set; }
        public string Building { get; set; } // Make sure this is a string
        public string RoomType { get; set; }
    }
}
