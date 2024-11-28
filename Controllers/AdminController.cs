using Eproject.Areas.Identity.Data;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AdminController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        var caterers = _context.caterers
            .Include(c => c.CatererFoodtypes) 
            .ThenInclude(cf => cf.FoodType)    
            .ToList();

        return View(caterers);
    }

    public IActionResult Create()
    {

        ViewBag.FoodTypes = _context.foodtypes.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(viewCaterer caterer, List<int> selectedFoodTypeIds)
    {
        if (selectedFoodTypeIds == null || !selectedFoodTypeIds.Any())
        {
            ModelState.AddModelError("selectedFoodTypeIds", "At least one food type must be selected.");
        }

        if (ModelState.IsValid)
        {
            // Image upload handling
            string imagePath = null;
            if (caterer.Photo != null)
            {
                var fileName = Path.GetRandomFileName() + Path.GetExtension(caterer.Photo.FileName);
                var folderPath = Path.Combine(_env.WebRootPath, "Images");
                var fullPath = Path.Combine(folderPath, fileName);

                Directory.CreateDirectory(folderPath); // Ensure folder exists
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    caterer.Photo.CopyTo(stream);
                }

                imagePath = $"/Images/{fileName}";
            }

            // Create new caterer object
            var newCaterer = new Caterer
            {
                Name = caterer.Name,
                Place = caterer.Place,
                MaxPeople = caterer.MaxPeople,
                Description = caterer.Description,
                imagePath = imagePath, // Use imagePath here
                PricePerPerson = caterer.PricePerPerson,
            };

            _context.caterers.Add(newCaterer);
            _context.SaveChanges();

            // Add selected food types
            if (selectedFoodTypeIds != null && selectedFoodTypeIds.Any())
            {
                foreach (var foodTypeId in selectedFoodTypeIds)
                {
                    var catererFoodType = new CatererFoodtype
                    {
                        CatererId = newCaterer.CatererId,
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
            .FirstOrDefault(c => c.CatererId == id);

        if (caterer == null)
        {
            return NotFound();
        }

        var viewCaterer = new viewCaterer
        {
            CatererId = caterer.CatererId,
            Name = caterer.Name,
            Place = caterer.Place,
            MaxPeople = caterer.MaxPeople,
            Description = caterer.Description,
            existingImagePath = caterer.imagePath, 
            PricePerPerson = caterer.PricePerPerson,
            SelectedFoodTypeIds = caterer.CatererFoodtypes.Select(cf => cf.FoodTypeId).ToList()
        };

        ViewBag.FoodTypes = _context.foodtypes.ToList(); 
        return View(viewCaterer);
    }


    [HttpPost]
    public IActionResult Edit(viewCaterer viewCaterer)
    {
        var caterer = _context.caterers
            .Include(c => c.CatererFoodtypes)
            .FirstOrDefault(c => c.CatererId == viewCaterer.CatererId);

        if (caterer == null)
        {
            return NotFound();
        }

        caterer.Name = viewCaterer.Name;
        caterer.Place = viewCaterer.Place;
        caterer.MaxPeople = viewCaterer.MaxPeople;
        caterer.Description = viewCaterer.Description;
        caterer.PricePerPerson = viewCaterer.PricePerPerson;

        if (viewCaterer.Photo != null)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Path.GetFileName(viewCaterer.Photo.FileName));
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                viewCaterer.Photo.CopyTo(stream);
            }
            caterer.imagePath = $"/images/{Path.GetFileName(viewCaterer.Photo.FileName)}";
        }
        else
        {
            caterer.imagePath = viewCaterer.existingImagePath; 
        }

        caterer.CatererFoodtypes.Clear();
        if (viewCaterer.SelectedFoodTypeIds != null)
        {
            foreach (var foodTypeId in viewCaterer.SelectedFoodTypeIds)
            {
                caterer.CatererFoodtypes.Add(new CatererFoodtype
                {
                    CatererId = caterer.CatererId,
                    FoodTypeId = foodTypeId
                });
            }
        }

        _context.SaveChanges();
        TempData["msg"] = "Caterer updated successfully.";
        return RedirectToAction("Index");
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
