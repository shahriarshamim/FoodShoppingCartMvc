using Microsoft.EntityFrameworkCore;

namespace FoodShoppingCartMvc.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock?> GetStockByItemId(int itemId) => await _context.Stocks.FirstOrDefaultAsync(s => s.ItemId == itemId);

        public async Task ManageStock(StockDTO stockToManage)
        {
            // if there is no stock for given book id, then add new record
            // if there is already stock for given book id, update stock's quantity
            var existingStock = await GetStockByItemId(stockToManage.ItemId);
            if (existingStock is null)
            {
                var stock = new Stock { ItemId = stockToManage.ItemId, Quantity = stockToManage.Quantity };
                _context.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from item in _context.Items
                                join stock in _context.Stocks
                                on item.Id equals stock.ItemId
                                into item_stock
                                from itemStock in item_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || item.ItemName.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    ItemId = item.Id,
                                    ItemName = item.ItemName,
                                    Quantity = itemStock == null ? 0 : itemStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }

    }
}
