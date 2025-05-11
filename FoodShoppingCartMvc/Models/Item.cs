using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShoppingCartMvc.Models
{
    [Table("Item")]
    public class Item
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
        public Category Category { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        public Stock Stock { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }


    }
}
