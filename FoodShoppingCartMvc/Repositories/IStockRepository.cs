namespace FoodShoppingCartMvc.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByItemId(int itemId);
        Task ManageStock(StockDTO stockToManage);
    }
}
