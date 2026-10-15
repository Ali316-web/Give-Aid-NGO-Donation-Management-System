using Give_Aid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiveAid.Controllers
{
    // Public-facing About Us page. If your project already has a controller
    // that serves about.html (e.g. an action on HomeController), move this
    // Index method there instead of adding a second route for the same page.
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }

        // GET: About
        public async Task<IActionResult> Index()
        {
            var vm = new AboutPageViewModel
            {
                Sections = await _context.AboutSections
                    .Where(s => s.IsVisible)
                    .ToListAsync(),
                TeamMembers = await _context.TeamMembers
                    .OrderBy(t => t.DisplayOrder)
                    .ToListAsync()
            };

            return View(vm);
        }
    }
}