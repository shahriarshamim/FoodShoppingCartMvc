using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodShoppingCartMvc.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }
        public List<Item> Items { get; set; }
    }
}
