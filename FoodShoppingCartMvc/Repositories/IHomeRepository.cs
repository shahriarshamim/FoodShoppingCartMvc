using FoodShoppingCartMvc.Models;

namespace FoodShoppingCartMvc.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Item>> GetItems(string sTerm = "", int categoryId = 0);
        Task<IEnumerable<Category>> Categories();
    }
}

