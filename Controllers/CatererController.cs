using Eproject.Areas.Identity.Data;
using Eproject.Migrations;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Eproject.Controllers
{
    public class CatererController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatererController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// Search Caterers
        public IActionResult Search(string searchString)
        {
            var caterers = _context.caterers.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                caterers = caterers.Where(c => c.Place.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                caterers = caterers.Where(c => c.CatererFoodtypes.Any(cf => cf.FoodType.FoodTypeName.Contains(searchString)));
            }

            //if (searchString)
            //{
            //    caterers = caterers.Where(c => c.MaxPeople >= searchString);
            //}

            var catererList = caterers.ToList();

            ViewBag.Caterers = catererList;

            return View(catererList);
        }



        // Caterer Details
        public IActionResult Details(int id)
        {
            var caterer = _context.caterers
                .Include(c => c.CatererFoodtypes)
                    .ThenInclude(cf => cf.FoodType)
                .FirstOrDefault(c => c.CatererId == id);

            if (caterer == null)
            {
                return NotFound();
            }

            var viewModel = new viewCaterer
            {
                CatererId = caterer.CatererId,
                Name = caterer.Name,
                existingImagePath = caterer.imagePath,
                Place = caterer.Place,
                MaxPeople = caterer.MaxPeople,
                PricePerPerson = caterer.PricePerPerson,
                Description = caterer.Description,
                CatererFoodtypes = caterer.CatererFoodtypes
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create(int? bookingId)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User is not logged in. Please log in and try again.";
                return RedirectToAction("Login", "Account");
            }

            // Fetch available caterers
            var adminCaterers = _context.caterers.Select(c => new SelectCatererViewModel
            {
                CatererId = c.CatererId.ToString(),
                Name = c.Name,
                Source = "Admin"
            }).ToList();

            var registeredCaterers = _context.Users
                .Where(u => u.Role == "Caterer")
                .Select(u => new SelectCatererViewModel
                {
                    CatererId = u.Id.ToString(),
                    Name = u.Name,
                    Source = "Registered"
                }).ToList();

            var allCaterers = adminCaterers.Concat(registeredCaterers).ToList();
            var foodTypes = _context.foodtypes.ToList();

            // Initialize model
            var model = new viewCategorymenuModel
            {
                caterers = allCaterers.Select(c => new Caterer
                {
                    CatererId = int.TryParse(c.CatererId, out int id) ? id : 0,
                    Name = c.Name
                }).ToList(),
                foodtypes = foodTypes.Select(f => new Foodtype
                {
                    FoodTypeId = f.FoodTypeId,
                    FoodTypeName = f.FoodTypeName
                }).ToList(),
                Booking = new Booking
                {
                    CustomerName = user.Name,
                    CustomerEmail = user.Email,
                    Venue = user.Address
                }
            };

            if (bookingId.HasValue)
            {
                var booking = _context.Bookings
                    .Include(b => b.BookingFoodTypes)
                    .ThenInclude(bf => bf.Foodtype)
                    .FirstOrDefault(b => b.BookingId == bookingId.Value);

                if (booking != null)
                {
                    model.Booking = booking;
                    model.SelectedFoodTypeIds = booking.BookingFoodTypes.Select(bf => bf.FoodTypeId).ToList();
                }
                else
                {
                    TempData["ErrorMessage"] = "Booking not found.";
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(viewCategorymenuModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            if (model.SelectedFoodTypeNames == null || !model.SelectedFoodTypeNames.Any())
            {
                TempData["ErrorMessage"] = "Please select at least one food type.";
                return View(model);
            }

            try
            {
                if (model.Booking.BookingId > 0) // Update existing booking
                {
                    var existingBooking = _context.Bookings
                        .Include(b => b.BookingFoodTypes)
                        .FirstOrDefault(b => b.BookingId == model.Booking.BookingId);

                    if (existingBooking != null)
                    {
                        existingBooking.Venue = model.Booking.Venue;
                        existingBooking.CustomerName = model.Booking.CustomerName;
                        existingBooking.CustomerPhone = model.Booking.CustomerPhone;
                        existingBooking.CustomerEmail = model.Booking.CustomerEmail;
                        existingBooking.BookingDate = model.Booking.BookingDate;
                        existingBooking.CatererId = model.Booking.CatererId;

                        // Update food types
                        existingBooking.BookingFoodTypes.Clear(); // Clear existing food types before adding new ones

                        foreach (var foodTypeName in model.SelectedFoodTypeNames)
                        {
                            var foodType = _context.foodtypes.FirstOrDefault(f => f.FoodTypeName == foodTypeName);
                            if (foodType != null)
                            {
                                existingBooking.BookingFoodTypes.Add(new BookingFoodType
                                {
                                    FoodTypeId = foodType.FoodTypeId
                                });
                            }
                        }

                        _context.Update(existingBooking);
                    }
                }
                else // Create new booking
                {
                    var newBooking = new Booking
                    {
                        Venue = model.Booking.Venue,
                        CustomerName = model.Booking.CustomerName,
                        CustomerPhone = model.Booking.CustomerPhone,
                        CustomerEmail = model.Booking.CustomerEmail,
                        BookingDate = model.Booking.BookingDate,
                        CatererId = model.Booking.CatererId
                    };

                    foreach (var foodTypeName in model.SelectedFoodTypeNames)
                    {
                        var foodType = _context.foodtypes.FirstOrDefault(f => f.FoodTypeName == foodTypeName);
                        if (foodType != null)
                        {
                            newBooking.BookingFoodTypes.Add(new BookingFoodType
                            {
                                FoodTypeId = foodType.FoodTypeId
                            });
                        }
                    }

                    _context.Bookings.Add(newBooking);
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Booking saved successfully!";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return View(model);
            }
        }




        public async Task<IActionResult> BookingList()
        {
            string currentUserEmail = User.Identity?.Name;

            var bookings = await _context.Bookings
                .Include(b => b.BookingFoodTypes)
                    .ThenInclude(bf => bf.Foodtype)
                .Include(b => b.Caterer)
                .Where(b => b.CustomerEmail == currentUserEmail)
                .ToListAsync();

            var bookingList = bookings.Select(b => new Booking
            {
                BookingId = b.BookingId,
                Venue = b.Venue,
                CustomerName = b.CustomerName,
                CustomerEmail = b.CustomerEmail,
                CustomerPhone = b.CustomerPhone,
                BookingDate = b.BookingDate,
                CatererName = b.Caterer.Name,
                SelectedFoodTypeNames = b.BookingFoodTypes.Select(bf => bf.Foodtype.FoodTypeName).ToList()
            }).ToList();

            return View(bookingList);
        }
        public async Task<IActionResult> View(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingFoodTypes)
                    .ThenInclude(bf => bf.Foodtype)
                .Include(b => b.Caterer)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var viewModel = new Booking
            {
                BookingId = booking.BookingId,
                Venue = booking.Venue,
                CustomerName = booking.CustomerName,
                CustomerEmail = booking.CustomerEmail,
                CustomerPhone = booking.CustomerPhone,
                BookingDate = booking.BookingDate,
                CatererName = booking.Caterer.Name,
                Status = booking.Status,
                SelectedFoodTypeNames = booking.BookingFoodTypes.Select(bf => bf.Foodtype.FoodTypeName).ToList()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BookingList));
        }
        [HttpPost]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();

            TempData["Message"] = "Booking has been successfully canceled.";
            return RedirectToAction(nameof(BookingList)); 
        }


    }
}

