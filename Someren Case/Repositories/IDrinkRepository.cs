using Someren_Case.Models;
using System.Collections.Generic;

namespace Someren_Case.Repositories
{
    public interface IDrinkRepository
    {
        List<Drink> GetAllDrinks();
        void AddDrinkOrder(DrinkOrder order);
    }
}