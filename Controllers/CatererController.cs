using Eproject.Areas.Identity.Data;
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
        public IActionResult ListCaterers()
        {
            return View();
        }
       

    }
}
