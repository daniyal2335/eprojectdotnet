using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int CatererId { get; set; }
        public DateTime BookingDate { get; set; }
        public string Venue { get; set; }
        public string Menu { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}
