using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FoodShoppingCartMvc.Models.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ItemName { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Weight { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
