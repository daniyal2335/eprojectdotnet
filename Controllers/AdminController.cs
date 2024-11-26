using Eproject.Areas.Identity.Data;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var caterers = _context.caterers
            .Include(c => c.CatererFoodtypes)  // Include the related food types
            .ThenInclude(cf => cf.FoodType)    // Include the foodtype from the join table
            .ToList();

        return View(caterers);
    }

    public IActionResult Create()
    {

        // Fetch all food types to display in the view
        ViewBag.FoodTypes = _context.foodtypes.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Caterer caterer, List<int> selectedFoodTypeIds)
    {
        if (selectedFoodTypeIds == null || !selectedFoodTypeIds.Any())
        {
            ModelState.AddModelError("selectedFoodTypeIds", "At least one food type must be selected.");
        }

        if (ModelState.IsValid)
        {
            _context.caterers.Add(caterer);
            _context.SaveChanges();  

            if (selectedFoodTypeIds != null && selectedFoodTypeIds.Any())
            {
                foreach (var foodTypeId in selectedFoodTypeIds)
                {
                    var catererFoodType = new CatererFoodtype
                    {
                        CatererId = caterer.CatererId,  
                        FoodTypeId = foodTypeId        
                    };
                    _context.CatererFoodtypes.Add(catererFoodType);  
                }
                _context.SaveChanges(); 
            }

            return RedirectToAction("Index"); 
        }

        ViewBag.FoodTypes = _context.foodtypes.ToList();
        return View(caterer); 
    }

    public IActionResult Edit(int id)
    {
        var caterer = _context.caterers
            .Include(c => c.CatererFoodtypes)
            .ThenInclude(cf => cf.FoodType)
            .FirstOrDefault(c => c.CatererId == id);

        if (caterer == null)
        {
            return NotFound(); 
        }

        ViewBag.FoodTypes = _context.foodtypes.ToList(); 
        var selectedFoodTypeIds = caterer.CatererFoodtypes.Select(cf => cf.FoodTypeId).ToList(); 

        ViewBag.SelectedFoodTypeIds = selectedFoodTypeIds;

        return View(caterer); 
    }
    [HttpPost]
    public IActionResult Edit(Caterer caterer, List<int> selectedFoodTypeIds)
    {
        if (selectedFoodTypeIds == null || !selectedFoodTypeIds.Any())
        {
            ModelState.AddModelError("selectedFoodTypeIds", "At least one food type must be selected.");
        }

        if (ModelState.IsValid)
        {
            var existingCaterer = _context.caterers
                .Include(c => c.CatererFoodtypes)
                .FirstOrDefault(c => c.CatererId == caterer.CatererId);

            if (existingCaterer == null)
            {
                return NotFound();
            }

            existingCaterer.Name = caterer.Name;
            existingCaterer.Place = caterer.Place;
            existingCaterer.MaxPeople = caterer.MaxPeople;
            existingCaterer.Description = caterer.Description;
            existingCaterer.PricePerPerson = caterer.PricePerPerson;

            _context.CatererFoodtypes.RemoveRange(existingCaterer.CatererFoodtypes);

            foreach (var foodTypeId in selectedFoodTypeIds)
            {
                var catererFoodType = new CatererFoodtype
                {
                    CatererId = existingCaterer.CatererId,
                    FoodTypeId = foodTypeId
                };
                _context.CatererFoodtypes.Add(catererFoodType);
            }

            _context.SaveChanges(); 
            return RedirectToAction("Index"); 
        }

        ViewBag.FoodTypes = _context.foodtypes.ToList();
        ViewBag.SelectedFoodTypeIds = selectedFoodTypeIds;

        return View(caterer); 
    }
    public IActionResult Delete(int id)
    {
        var caterer = _context.caterers
            .Include(c => c.CatererFoodtypes) 
            .ThenInclude(cf => cf.FoodType) 
            .FirstOrDefault(c => c.CatererId == id);

        if (caterer == null)
        {
            return NotFound();  
        }

        return View(caterer); 
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var caterer = _context.caterers
            .Include(c => c.CatererFoodtypes) 
            .FirstOrDefault(c => c.CatererId == id);

        if (caterer == null)
        {
            return NotFound();  
        }

        _context.CatererFoodtypes.RemoveRange(caterer.CatererFoodtypes);

        _context.caterers.Remove(caterer);

        _context.SaveChanges();  

        return RedirectToAction("Index");  
    }






}
