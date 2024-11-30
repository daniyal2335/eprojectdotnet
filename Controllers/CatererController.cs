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

            // Apply filters based on user input
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

            // Execute the query and get the results
            var catererList = caterers.ToList();

            // Pass the filtered list to the view
            ViewBag.Caterers = catererList;

            // Return the search results view
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

            // Map to ViewModel if needed
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
        // Get the booking form (GET request)
        public IActionResult Create(int? bookingId)
        {
            // Fetch logged-in user details
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier); // Get UserId from claims
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (userId == null)
            {
                TempData["ErrorMessage"] = "User is not logged in. Please log in and try again.";
                return RedirectToAction("Login", "Account");
            }

            // Fetch admin-added caterers
            var adminCaterers = _context.caterers.Select(c => new SelectCatererViewModel
            {
                CatererId = c.CatererId.ToString(),
                Name = c.Name,
                Source = "Admin"
            }).ToList();

            // Fetch registered caterers
            var registeredCaterers = _context.Users
                .Where(u => u.Role == "Caterer")
                .Select(u => new SelectCatererViewModel
                {
                    CatererId = u.Id.ToString(),
                    Name = u.Name,
                    Source = "Registered"
                }).ToList();

            // Combine both caterer lists
            var allCaterers = adminCaterers.Concat(registeredCaterers).ToList();
            var foodTypes = _context.foodtypes.ToList();
            // Create the model
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

            // If we have a booking ID, load the booking details for editing
            if (bookingId.HasValue)
            {
                var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId.Value);
                if (booking != null)
                {
                    model.Booking = booking;
                    model.SelectedFoodTypeIds = booking.SelectedFoodTypes.Select(ft => ft.FoodTypeId).ToList();// Populate the model with the existing booking details
                }
                else
                {
                    TempData["ErrorMessage"] = "Booking not found.";
                }
            }

            return View(model); // Pass the model to the view
        }

        // Submit the booking (POST request)
        [HttpPost]
        public IActionResult Create(viewCategorymenuModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form validation failed. Please check the fields and try again.";
                return View(model);
            }

            try
            {
                // If BookingId exists, update the existing booking
                if (model.Booking.BookingId > 0)
                {
                    var existingBooking = _context.Bookings.Include(b => b.SelectedFoodTypes)
                                                            .FirstOrDefault(b => b.BookingId == model.Booking.BookingId);
                    if (existingBooking == null)
                    {
                        TempData["ErrorMessage"] = "Booking not found.";
                        return View(model);
                    }

                    // Ensure SelectedFoodTypes is initialized
                    if (existingBooking.SelectedFoodTypes == null)
                    {
                        existingBooking.SelectedFoodTypes = new List<Foodtype>();
                    }

                    // Update the existing booking
                    existingBooking.Venue = model.Booking.Venue;
                    existingBooking.CustomerName = model.Booking.CustomerName;
                    existingBooking.CustomerPhone = model.Booking.CustomerPhone;
                    existingBooking.CustomerEmail = model.Booking.CustomerEmail;
                    existingBooking.BookingDate = model.Booking.BookingDate;
                    existingBooking.CatererId = model.Booking.CatererId;

                    // Update selected food types
                    existingBooking.SelectedFoodTypes.Clear();
                    foreach (var foodtypeId in model.Booking.SelectedFoodTypeIds)
                    {
                        var foodtype = _context.foodtypes.FirstOrDefault(f => f.FoodTypeId == foodtypeId);
                        if (foodtype != null) // Only add if foodtype is found
                        {
                            existingBooking.SelectedFoodTypes.Add(foodtype);
                        }
                    }

                    _context.Update(existingBooking);
                    TempData["SuccessMessage"] = "Booking updated successfully!";
                }
                else
                {
                    // If no BookingId, create a new booking
                    var newBooking = new Booking
                    {
                        Venue = model.Booking.Venue,
                        CustomerName = model.Booking.CustomerName,
                        CustomerPhone = model.Booking.CustomerPhone,
                        CustomerEmail = model.Booking.CustomerEmail,
                        BookingDate = model.Booking.BookingDate,
                        CatererId = model.Booking.CatererId
                    };

                    // Ensure SelectedFoodTypeIds is not null
                    if (model.Booking.SelectedFoodTypeIds == null)
                    {
                        model.Booking.SelectedFoodTypeIds = new List<int>();
                    }

                    // Add selected food types
                    foreach (var foodtypeId in model.Booking.SelectedFoodTypeIds)
                    {
                        var foodtype = _context.foodtypes.FirstOrDefault(f => f.FoodTypeId == foodtypeId);
                        if (foodtype != null) // Only add if foodtype is found
                        {
                            newBooking.SelectedFoodTypes.Add(foodtype);
                        }
                    }

                    _context.Bookings.Add(newBooking);
                    TempData["SuccessMessage"] = "Booking created successfully!";
                }

                _context.SaveChanges();
                return RedirectToAction("Create"); // Redirect to avoid duplicate submissions
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return View(model);
            }
        }


        // GET: Booking Success Page
        public IActionResult BookingSuccess()
            {
                return View(); // Return a view that displays booking success confirmation
            }
        }
    }

