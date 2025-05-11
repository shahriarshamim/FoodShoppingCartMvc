using FoodShoppingCartMvc.Data;
using FoodShoppingCartMvc.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace FoodShoppingCartMvc.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Categories()
        {            
            return await _db.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Item>> GetItems(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Item> items = await (from item in _db.Items
                                             join category in _db.Categories
                                             on item.CategoryId equals category.Id
                                             join stock in _db.Stocks
                                             on item.Id equals stock.ItemId
                                             into item_stocks
                                             from itemWithStock in item_stocks.DefaultIfEmpty()
                                             where string.IsNullOrWhiteSpace(sTerm) || (item != null && item.ItemName.ToLower().StartsWith(sTerm))
                                             select new Item
                                             {
                                                 Id = item.Id,
                                                 Image = item.Image,
                                                 Weight = item.Weight,
                                                 ItemName = item.ItemName,
                                                 CategoryId = item.CategoryId,
                                                 Price = item.Price,
                                                 CategoryName = category.CategoryName,
                                                 Quantity = itemWithStock == null ? 0 : itemWithStock.Quantity
                                             }
                         ).ToListAsync();
            if (categoryId > 0)
            {

                items = items.Where(a => a.CategoryId == categoryId).ToList();
            }
            return items;

        }
    }
}
