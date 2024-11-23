using System;
using System.Collections.Generic;

namespace Eproject.Models;

public partial class Caterer
{
    public int CatererId { get; set; }

    public string Name { get; set; } = null!;

    public string Place { get; set; } = null!;

    public int MaxPeople { get; set; }

    public string Description { get; set; } = null!;

    public int PricePerPerson { get; set; }

    public int? FoodTypeId { get; set; }
}
