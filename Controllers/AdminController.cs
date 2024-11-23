using Eproject.Areas.Identity.Data;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject.Controllers
{
  
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List Caterers
        public IActionResult Index()
        {
            var caterers = _context.caterers.ToList();
            return View(caterers);
        }

        // Add New Caterer
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Caterers caterer)
        {
            if (ModelState.IsValid)
            {
                _context.caterers.Add(caterer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caterer);
        }

        // Edit Caterer
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var caterer = _context.caterers.FirstOrDefault(c => c.CatererId == id);
            if (caterer == null)
                return NotFound();
            return View(caterer);
        }

        [HttpPost]
        public IActionResult Edit(Caterers caterer)
        {
            if (ModelState.IsValid)
            {
                _context.caterers.Update(caterer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caterer);
        }

        // Delete Caterer
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var caterer = _context.caterers.FirstOrDefault(c => c.CatererId == id);
            if (caterer == null)
                return NotFound();
            return View(caterer);
        }

        [HttpPost]
        public IActionResult Delete(Caterers caterer)
        {
            _context.caterers.Remove(caterer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
