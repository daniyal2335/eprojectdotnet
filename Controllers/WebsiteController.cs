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
        public IActionResult Home()
        {
            var cat=_context.Categories.ToList();
            var menu = _context.MenuItems.ToList();
            var viewModel = new viewCategorymenuModel
            {
                Categories = cat,
                MenuItems=menu
            };
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
        public IActionResult Search()
        {
            return View();
        }
     
    }
}
