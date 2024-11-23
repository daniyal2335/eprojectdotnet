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
        [Required]
        public string Place { get; set; }
        [Required]
        public int MaxPeople { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal PricePerPerson{get; set;}
        public int FoodTypeid {  get; set; }
        public Foodtype Foodtype { get; set; }
       



    } 
}
