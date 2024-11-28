using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }  

        [Required]
        public DateTime BookingDate { get; set; } 

        [Required]
        [MaxLength(255)]
        public string Venue { get; set; } 

        [Required]
        [MaxLength(255)]
        public string CustomerName { get; set; } 

        [MaxLength(15)]
        public string CustomerPhone { get; set; } = null!;  

        [MaxLength(255)]
        public string CustomerEmail { get; set; } = null!;

        public int CatererId { get; set; } 

        public  Caterer Caterer { get; set; }
    }
}
