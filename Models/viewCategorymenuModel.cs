namespace Eproject.Models
{
    public class viewCategorymenuModel
    {
        public  IEnumerable<Category> Categories { get; set; }
        public  IEnumerable<MenuItem> MenuItems { get; set; }
        public IEnumerable<Foodtype> foodtypes { get; set; }  
        public int SelectedCategoryId { get; set; }
    }
}
