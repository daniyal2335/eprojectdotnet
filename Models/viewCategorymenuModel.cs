
namespace Eproject.Models
{
    public class viewCategorymenuModel
    {
        public  IEnumerable<Category> Categories { get; set; }
        public  IEnumerable<MenuItem> MenuItems { get; set; }
        public IEnumerable<Foodtype> foodtypes { get; set; }
        public IEnumerable<Caterer> caterers { get; set; }

        public int SelectedCategoryId { get; set; }
        public Booking Booking { get; set; }
        public List<int> SelectedFoodTypeIds { get; internal set; } = new List<int>();
        // New field to store selected food type names (for booking purposes)
        public List<string> SelectedFoodTypeNames { get; internal set; } = new List<string>();

    }
}
