﻿namespace FoodShoppingCartMvc.Models.DTOs
{
    public class StockDisplayModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string? ItemName { get; set; }
    }
}
