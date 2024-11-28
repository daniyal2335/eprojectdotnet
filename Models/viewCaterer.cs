using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class viewCaterer
    {
        internal string existingImagePath;
        public int CatererId { get; set; }

        public string Name { get; set; } = null!;

        public string Place { get; set; } = null!;

        public int MaxPeople { get; set; }

        public string Description { get; set; } = null!;
        public IFormFile Photo { get; set; }


        public int PricePerPerson { get; set; }
        [Required]
        public ICollection<CatererFoodtype> CatererFoodtypes { get; set; } = new List<CatererFoodtype>();
        public ICollection<int> SelectedFoodTypeIds { get; set; } = new List<int>();
    }
}
