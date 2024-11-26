using Eproject.Areas.Identity.Data;
using Eproject.Migrations;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WebsiteController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Home(int selectedCategoryId = 0)
        {
            var categories = _context.Categories.ToList();

            // Default to the first category if none is selected
            if (selectedCategoryId == 0 && categories.Any())
            {
                selectedCategoryId = categories.First().CategoryId;
            }

            var menuItems = _context.MenuItems
                                    .Where(m => m.CategoryId == selectedCategoryId || selectedCategoryId == 0)
                                    .ToList();
            var foodTypes = _context.foodtypes.ToList();

            var viewModel = new viewCategorymenuModel
            {
                Categories = categories,
                MenuItems = menuItems,
                SelectedCategoryId = selectedCategoryId,
                foodtypes = foodTypes
            };

            ViewData["selectedCategoryId"] = selectedCategoryId;
            return View(viewModel);
        }




        public IActionResult Menu()
        {

            return View();
        }
        public IActionResult Events()
        {
            return View();
        }
        public IActionResult Aboutus()
        {
            return View();
        }
        public IActionResult Pages()
        {
            return View();
        }
        public IActionResult Contactus()
        {
            return View();
        }
      
     
    }
}
