using Eproject.Migrations;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Caterers
    {
        [Key]
        public int CatererId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Place { get; set; }
        public string FoodType { get; set; }
        public int MaxPeople { get; set; }
        public string Description { get; set; }
        public decimal PricePerPerson{get; set;}
    
    } 
}
