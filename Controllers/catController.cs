    using Eproject.Areas.Identity.Data;
    using Eproject.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace Eproject.Controllers
    {
        public class catController : Controller
        {
            private readonly ApplicationDbContext _context;

            public catController(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Index()
            {
                return View(await _context.Categories.ToListAsync());
            }
            public IActionResult AddCategory()
            {
                    return View();
            }
            [HttpPost]
            public async Task<IActionResult> AddCategory(Category cat)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var a = _context.Categories.Where(a => a.CategoryName == cat.CategoryName).FirstOrDefault();
                        if (a != null)
                        {
                            ViewBag.msg = "Category is available";
                        }
                        else
                        {
                            _context.Categories.Add(cat);
                            if (await _context.SaveChangesAsync() > 0)
                            {
                                return RedirectToAction("Index");
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.msg = e;

                    }
                }
                return View();
            }


            public IActionResult edit(int id)
            {
                var a = _context.Categories.Find(id);
                return View(a);
            }
            [HttpPost]
            public async Task<IActionResult> edit(int id, Category c)
            {

                try
                {
                    var a = _context.Categories.Find(id);
                    var check = _context.Categories.Where(a => a.CategoryName == c.CategoryName && a.CategoryId != id).FirstOrDefault();

                    if (check != null)
                    {
                        ViewBag.msg = "category is available";
                        return View(a);


                    }
                    else
                    {

                        a.CategoryName = c.CategoryName;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception e)
                {
                    ViewBag.msg = e;
                }
                return RedirectToAction("Index");

            }
            public IActionResult delete(int id)
            {
                var a = _context.Categories.Find(id);
                _context.Categories.Remove(a);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
