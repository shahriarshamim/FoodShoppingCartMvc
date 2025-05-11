﻿using System.ComponentModel.DataAnnotations;

namespace FoodShoppingCartMvc.Models.DTOs
{
    public class StockDTO
    {
        public int ItemId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; }
    }
}
