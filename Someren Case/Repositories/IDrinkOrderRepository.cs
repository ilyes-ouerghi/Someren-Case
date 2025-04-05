namespace Someren_Case.Interfaces
{
    public interface IDrinkOrderRepository
    {
        void AddDrinkOrder(int studentId, int drinkId, int quantity);
    }
}