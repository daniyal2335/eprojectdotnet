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
        // Navigation property for CatererFoodtypes
        public ICollection<CatererFoodtype> CatererFoodtypes { get; set; }

        public ICollection<BookingFoodType> BookingFoodTypes { get; set; } = new List<BookingFoodType>();
    }
}
