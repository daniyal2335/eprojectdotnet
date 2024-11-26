using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models;

public partial class CatererFoodtype
{
    public int CatererFoodtypeId { get; set; }

    public int CatererId { get; set; }
    public Caterer Caterer { get; set; }

    public int FoodTypeId { get; set; }
    public Foodtype FoodType { get; set; }
}
