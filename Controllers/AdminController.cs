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
        // Ensure that the model is valid before proceeding
        if (selectedFoodTypeIds == null || !selectedFoodTypeIds.Any())
        {
            // If no food types are selected, add an error to the ModelState (optional)
            ModelState.AddModelError("selectedFoodTypeIds", "At least one food type must be selected.");
        }

        if (ModelState.IsValid)
        {
            // Add the caterer to the database
            _context.caterers.Add(caterer);
            _context.SaveChanges();  // Save caterer to get CatererId

            // If food types are selected, add them to the join table (CatererFoodtype)
            if (selectedFoodTypeIds != null && selectedFoodTypeIds.Any())
            {
                foreach (var foodTypeId in selectedFoodTypeIds)
                {
                    var catererFoodType = new CatererFoodtype
                    {
                        CatererId = caterer.CatererId,  // Assign CatererId to the join table
                        FoodTypeId = foodTypeId        // Assign selected FoodTypeId to the join table
                    };
                    _context.CatererFoodtypes.Add(catererFoodType);  // Add to the join table
                }
                _context.SaveChanges();  // Save food types
            }

            return RedirectToAction("Index"); // Redirect to the index page after successful creation
        }

        // If validation fails, reload the list of food types to display again
        ViewBag.FoodTypes = _context.foodtypes.ToList();
        return View(caterer); // Return to view with errors
    }

    public IActionResult Edit(int id)
    {
        var caterer = _context.caterers
            .Include(c => c.CatererFoodtypes)
            .ThenInclude(cf => cf.FoodType)
            .FirstOrDefault(c => c.CatererId == id);

        if (caterer == null)
        {
            return NotFound(); // Return 404 if caterer not found
        }

        ViewBag.FoodTypes = _context.foodtypes.ToList(); // Load food types for selection
        var selectedFoodTypeIds = caterer.CatererFoodtypes.Select(cf => cf.FoodTypeId).ToList(); // Get selected food types

        // Pass caterer and selected food type IDs directly to the view
        ViewBag.SelectedFoodTypeIds = selectedFoodTypeIds;

        return View(caterer); // Directly pass the caterer to the view
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

            // Update the existing caterer details
            existingCaterer.Name = caterer.Name;
            existingCaterer.Place = caterer.Place;
            existingCaterer.MaxPeople = caterer.MaxPeople;
            existingCaterer.Description = caterer.Description;
            existingCaterer.PricePerPerson = caterer.PricePerPerson;

            // Remove existing food type associations
            _context.CatererFoodtypes.RemoveRange(existingCaterer.CatererFoodtypes);

            // Add new food type associations
            foreach (var foodTypeId in selectedFoodTypeIds)
            {
                var catererFoodType = new CatererFoodtype
                {
                    CatererId = existingCaterer.CatererId,
                    FoodTypeId = foodTypeId
                };
                _context.CatererFoodtypes.Add(catererFoodType);
            }

            _context.SaveChanges(); // Save the changes to the database
            return RedirectToAction("Index"); // Redirect to the index page
        }

        // Reload the food types if validation fails
        ViewBag.FoodTypes = _context.foodtypes.ToList();
        ViewBag.SelectedFoodTypeIds = selectedFoodTypeIds;

        return View(caterer); // Return to the view with validation errors
    }
    public IActionResult Delete(int id)
    {
        // Fetch the caterer and related food types to confirm the delete action
        var caterer = _context.caterers
            .Include(c => c.CatererFoodtypes) // Include related food types
            .ThenInclude(cf => cf.FoodType)  // Include FoodType data
            .FirstOrDefault(c => c.CatererId == id);

        if (caterer == null)
        {
            return NotFound();  // If the caterer doesn't exist, return 404
        }

        return View(caterer);  // Pass the caterer data to the view for confirmation
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var caterer = _context.caterers
            .Include(c => c.CatererFoodtypes) // Include related food types for removal
            .FirstOrDefault(c => c.CatererId == id);

        if (caterer == null)
        {
            return NotFound();  // If the caterer doesn't exist, return 404
        }

        // Remove related food types from the join table
        _context.CatererFoodtypes.RemoveRange(caterer.CatererFoodtypes);

        // Remove the caterer itself
        _context.caterers.Remove(caterer);

        _context.SaveChanges();  // Save changes to the database

        return RedirectToAction("Index");  // Redirect to the list of caterers after deletion
    }






}
