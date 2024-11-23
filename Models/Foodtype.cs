using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Foodtype
    {
        [Key]
        public int FoodTypeId { get; set; }
        [Required]
        public string FoodTypeName { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public string imagePath {  get; set; }
    }
}
