using Eproject.Areas.Identity.Data;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eproject.Controllers
{
    public class FoodController : Controller
    {
        private readonly ApplicationDbContext _context;


        private readonly IWebHostEnvironment _env;
        public FoodController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var a = _context.foodtypes.ToList();
            
            return View(a);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(viewFoodType Food)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingItem = _context.foodtypes.FirstOrDefault(a => a.FoodTypeName == Food.FoodTypeName);
                    if (existingItem != null)
                    {
                        TempData["extError"] = "Item already exists";
                        return View(Food);
                    }

                    string filename = "";
                    if (Food.Photo != null)
                    {
                        var ext = Path.GetExtension(Food.Photo.FileName).ToLower();
                        var size = Food.Photo.Length;

                        if ((ext == ".jpg" || ext == ".png" || ext == ".webp" || ext == ".jpeg") && size < 1000000)
                        {
                            string folder = Path.Combine(_env.WebRootPath, "Images");
                            filename = Path.GetRandomFileName() + ext;
                            string filepath = Path.Combine(folder, filename);

                            using (var fileStream = new FileStream(filepath, FileMode.Create))
                            {
                                await Food.Photo.CopyToAsync(fileStream);
                            }

                            Foodtype p = new Foodtype()
                            {
                               FoodTypeName = Food.FoodTypeName,
                                price = Food.price,
                                imagePath = filename,
                            };

                            _context.foodtypes.Add(p);
                            await _context.SaveChangesAsync();

                            ViewBag.msg = "Food Created Successfully!";
                            return Redirect("Index");
                        }
                        else
                        {
                            TempData["sizeErr"] = size >= 1000000 ? "Size must be less than 1MB" : "Invalid file format!";
                            return View(Food);
                        }
                    }
                    else
                    {
                        TempData["extError"] = "Please upload a valid image file!";
                        return View(Food);
                    }
                }
                catch (Exception e)
                {
                    TempData["extError"] = $"Error: {e.Message}";
                }
            }

            return View(Food);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var foodtype = await _context.foodtypes.FindAsync(id);
            if (foodtype == null)
            {
                return NotFound();
            }

            var viewModel = new viewFoodType
            {
                FoodTypeId = foodtype.FoodTypeId,
                FoodTypeName = foodtype.FoodTypeName,
                price = foodtype.price,
                existingImagePath = foodtype.imagePath,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(viewFoodType food)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var foodtype = await _context.foodtypes.FindAsync(food.FoodTypeId);
                    if (foodtype == null)
                    {
                        TempData["extError"] = "Item not found!";
                        return RedirectToAction(nameof(Edit), new { id = food.FoodTypeId });
                    }

                    // Check if there is another food type with the same name (excluding the current one)
                    var existingItem = await _context.foodtypes
                        .FirstOrDefaultAsync(a => a.FoodTypeName == food.FoodTypeName && a.FoodTypeId != food.FoodTypeId);

                    if (existingItem != null)
                    {
                        TempData["extError"] = "Another item with the same name already exists!";
                        return RedirectToAction(nameof(Edit), new { id = food.FoodTypeId });
                    }

                    string filename = foodtype.imagePath;
                    if (food.Photo != null)
                    {
                        var ext = Path.GetExtension(food.Photo.FileName).ToLower();
                        var size = food.Photo.Length;

                        if ((ext.Equals(".jpg") || ext.Equals(".png") || ext.Equals(".webp") || ext.Equals(".jpeg")) && size < 1000000)
                        {
                            string folder = Path.Combine(_env.WebRootPath, "Images");

                            filename = Guid.NewGuid().ToString() + ext;
                            string filepath = Path.Combine(folder, filename);

                            using (var fileStream = new FileStream(filepath, FileMode.Create))
                            {
                                await food.Photo.CopyToAsync(fileStream);
                            }

                            // Delete old image if it exists
                            if (!string.IsNullOrEmpty(foodtype.imagePath))
                            {
                                string oldFilePath = Path.Combine(folder, foodtype.imagePath);
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                            }
                        }
                        else
                        {
                            TempData["sizeErr"] = size >= 1000000 ? "Size must be less than 1MB" : "Invalid file format!";
                            return RedirectToAction(nameof(Edit), new { id = food.FoodTypeId });
                        }
                    }

                    // Update food type properties
                    foodtype.FoodTypeName = food.FoodTypeName;
                    foodtype.price = food.price;
                    foodtype.imagePath = filename;

                    _context.foodtypes.Update(foodtype);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Food updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["extError"] = $"Error: {e.Message}";
                    return RedirectToAction(nameof(Edit), new { id = food.FoodTypeId });
                }
            }

            return View(food);
        }



        public IActionResult delete(int id)
        {
            var a = _context.foodtypes.Find(id);
            _context.foodtypes.Remove(a);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
