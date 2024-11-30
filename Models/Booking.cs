using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        [MaxLength(255, ErrorMessage = "Venue cannot be more than 255 characters")]
        public string Venue { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [MaxLength(255, ErrorMessage = "Customer name cannot be more than 255 characters")]
        public string CustomerName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string CustomerEmail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Caterer is required")]
        public int CatererId { get; set; }
        // New Menu Field: List of FoodType Ids
        [Required(ErrorMessage = "Menu selection is required")]
        public List<int> SelectedFoodTypeIds { get; set; } = new List<int>(); // Default initialization

        // Navigation properties
        public ICollection<Foodtype> SelectedFoodTypes { get; set; } = new List<Foodtype>(); // Default initialization

    }
}
