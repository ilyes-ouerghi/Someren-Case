using Someren_Case.Models;
using System.Collections.Generic;

namespace Someren_Case.Interfaces
{
    public interface IDrinkRepository
    {
        List<Drink> GetAllDrinks();
        List<Student> GetAllStudents();
        void AddDrinkOrder(DrinkOrder order);
    }
}
