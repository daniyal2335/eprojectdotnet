using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Booking
    {
        internal object caterer;

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
        public virtual Caterer Caterer { get; set; }
        [Required(ErrorMessage = "Menu selection is required")]
        public List<string> SelectedFoodTypeNames { get; set; } = new List<string>();  

        public ICollection<BookingFoodType> BookingFoodTypes { get; set; } = new List<BookingFoodType>();
        [NotMapped]
        public string CatererName { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
