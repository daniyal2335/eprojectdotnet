using Eproject.Areas.Identity.Data;
using Eproject.Migrations;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
namespace Eproject.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
      
    
        private readonly IWebHostEnvironment _env;
        public MenuController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            _env = env;
        }
        public IActionResult Index(string searchString)
        {
            var a = _context.MenuItems.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                a = _context.MenuItems.Where(x => x.ItemName.Contains(searchString)).ToList();
            }
            var result = _context.MenuItems.Include(m => m.Category).ToList();
            return View(a);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(viewProductModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingItem = _context.MenuItems.FirstOrDefault(a => a.ItemName == product.ItemName);
                    if (existingItem != null)
                    {
                        TempData["extError"] = "Item already exists!";
                        return View(product); 
                    }

                    string filename = "";
                    if (product.photo != null)
                    {
                        var ext = Path.GetExtension(product.photo.FileName).ToLower();
                        var size = product.photo.Length;

                        if ((ext == ".jpg" || ext == ".png" || ext == ".webp" || ext == ".jpeg") && size < 1000000)
                        {
                            string folder = Path.Combine(_env.WebRootPath, "Images");
                            filename = Path.GetRandomFileName() + ext; 
                            string filepath = Path.Combine(folder, filename);

                            using (var fileStream = new FileStream(filepath, FileMode.Create))
                            {
                                await product.photo.CopyToAsync(fileStream);
                            }

                            MenuItem p = new MenuItem()
                            {
                                ItemName = product.ItemName,
                                Price = product.Price,
                                imagePath = filename,
                                CategoryId = product.CategoryId
                            };

                            _context.MenuItems.Add(p);
                            await _context.SaveChangesAsync();

                            ViewBag.msg = "Product Created Successfully!";
                            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                            return View();
                        }
                        else
                        {
                            TempData["sizeErr"] = size >= 1000000 ? "Size must be less than 1MB" : "Invalid file format!";
                            return View(product);
                        }
                    }
                    else
                    {
                        TempData["extError"] = "Please upload a valid image file!";
                        return View(product);
                    }
                }
                catch (Exception e)
                {
                    TempData["extError"] = $"Error: {e.Message}";
                }
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product); 
        }

        public IActionResult Edit(int id)
        {
            var menuItem = _context.MenuItems.Find(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            var viewModel = new viewProductModel
            {
                Id = menuItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Price = menuItem.Price,
                CategoryId = menuItem.CategoryId,
                existingImagePath = menuItem.imagePath 
            };

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", menuItem.CategoryId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(viewProductModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var menuItem = await _context.MenuItems.FindAsync(product.Id);
                    if (menuItem == null)
                    {
                        TempData["extError"] = "Item not found!";
                        return RedirectToAction(nameof(Edit), new { id = product.Id });
                    }

                    var existingItem = await _context.MenuItems
                        .FirstOrDefaultAsync(a => a.ItemName == product.ItemName && a.MenuItemId != product.Id);
                    if (existingItem != null)
                    {
                        TempData["extError"] = "Another item with the same name already exists!";
                        return RedirectToAction(nameof(Edit), new { id = product.Id });
                    }

                    string filename = menuItem.imagePath; 
                    if (product.photo != null)
                    {
                        var ext = Path.GetExtension(product.photo.FileName).ToLower();
                        var size = product.photo.Length;

                        if ((ext.Equals(".jpg") || ext.Equals(".png") || ext.Equals(".webp") || ext.Equals(".jpeg")) && size < 1000000)
                        {
                            string folder = Path.Combine(_env.WebRootPath, "Images");
                            filename = product.photo.FileName;
                            string filepath = Path.Combine(folder, filename);

                            using (var fileStream = new FileStream(filepath, FileMode.Create))
                            {
                                await product.photo.CopyToAsync(fileStream);
                            }

                            // Delete old image if it exists
                            if (!string.IsNullOrEmpty(menuItem.imagePath))
                            {
                                string oldFilePath = Path.Combine(folder, menuItem.imagePath);
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                            }
                        }
                        else
                        {
                            TempData["sizeErr"] = size >= 1000000 ? "Size must be less than 1MB" : "Invalid file format!";
                            return RedirectToAction(nameof(Edit), new { id = product.Id });
                        }
                    }

                    menuItem.ItemName = product.ItemName;
                    menuItem.Price = product.Price;
                    menuItem.CategoryId = product.CategoryId;
                    menuItem.imagePath = filename;

                    _context.MenuItems.Update(menuItem);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Product updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["extError"] = $"Error: {e.Message}";
                    return RedirectToAction(nameof(Edit), new { id = product.Id });
                }
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        public IActionResult delete(int id)
        {
            var a = _context.MenuItems.Find(id);
            _context.MenuItems.Remove(a);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

