using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class viewFoodType
    {
        internal string existingImagePath;
        [Key]
        public int FoodTypeId { get; set; }
        public string FoodTypeName { get; set; }
        public int price { get; set; }

        public IFormFile Photo { get; set; }
    }
}
