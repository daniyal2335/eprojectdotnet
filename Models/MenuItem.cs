using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }
        [Required]
        public string ItemName { get; set; }

        [Required]
        public int Price { get; set; }
        [Required]
        public string imagePath { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public string Des { get; set; }
    }
}
