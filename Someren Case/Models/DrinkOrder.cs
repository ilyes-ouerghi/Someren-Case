namespace Someren_Case.Models
{
    public class DrinkOrder
    {
        public int StudentID { get; set; }
        public int DrinkID { get; set; }
        public int Count { get; set; } // This maps to the Quantity column in OrderTable
    }
}