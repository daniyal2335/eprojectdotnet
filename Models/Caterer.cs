using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models;

public partial class Caterer
{
    public int CatererId { get; set; }

    public string Name { get; set; } = null!;

    public string Place { get; set; } = null!;

    public int MaxPeople { get; set; }

    public string Description { get; set; } = null!;
    public string imagePath { get; set; }

    public int PricePerPerson { get; set; }

    [Required]
    public ICollection<CatererFoodtype> CatererFoodtypes { get; set; } = new List<CatererFoodtype>();
}
