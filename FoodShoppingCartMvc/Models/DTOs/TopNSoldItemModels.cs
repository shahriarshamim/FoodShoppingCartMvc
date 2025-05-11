namespace FoodShoppingCartMvc.Models.DTOs;

    public record TopNSoldItemModel(string ItemName, string Weight, int TotalUnitSold);
    public record TopNSoldItemsVm(DateTime StartDate, DateTime EndDate, IEnumerable<TopNSoldItemModel> TopNSoldItems);

