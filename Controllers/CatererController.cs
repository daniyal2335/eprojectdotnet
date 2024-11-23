using Eproject.Areas.Identity.Data;
using Eproject.Migrations;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject.Controllers
{
    public class CatererController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatererController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Search Caterers
        //public IActionResult Search(string place, string foodType, int? people)
        //{
        //    var caterers = _context.caterers.AsQueryable();

        //    if (!string.IsNullOrEmpty(place))
        //        caterers = caterers.Where(c => c.Place.Contains(place));

        //    if (!string.IsNullOrEmpty(foodType))
        //        caterers = caterers.Where(c => c.Foodtype.Contains(foodType));

        //    if (people.HasValue)
        //        caterers = caterers.Where(c => c.MaxPeople >= people);

        //    var catererList = caterers.ToList(); 

        //    ViewBag.Caterers = catererList;

        //    return View(catererList);
        //}



        // Caterer Details
        //public IActionResult Details(int id)
        //{
        //    var caterer = _context.caterers.FirstOrDefault(c => c.CatererId == id);
        //    if (caterer == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(caterer);
        //}

        //// Booking
        //[HttpPost]
        //public IActionResult Book(Booking booking)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var caterer = _context.caterers.FirstOrDefault(c => c.CatererId == booking.CatererId);
        //        if (caterer == null) return NotFound();

        //        booking.TotalPrice = caterer.PricePerPerson * booking.Menu.Split(',').Length;

        //        _context.bookings.Add(booking);
        //        _context.SaveChanges();

        //        return RedirectToAction("BookingConfirmation", new { id = booking.BookingId });
        //    }

        //    return View(booking);
        //}


        //public IActionResult BookingConfirmation(int id)
        //{
        //    var booking = _context.bookings.FirstOrDefault(b => b.BookingId == id);
        //    if (booking == null) return NotFound();

        //    return View(booking);
        //}



    }
}
