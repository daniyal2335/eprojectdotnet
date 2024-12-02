using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class BookingFoodType
    {
        [Key]
        public int BookingFoodTypeId { get; set; }

        // Foreign key for Booking
        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        // Foreign key for Foodtype
        [ForeignKey("Foodtype")]
        public int FoodTypeId { get; set; }
        public Foodtype Foodtype { get; set; }
    }
}
