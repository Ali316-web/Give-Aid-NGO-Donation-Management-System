using Give_Aid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    // Public-facing Our Partners page. No [Authorize] — anyone can view it.
    public class PartnersController : Controller
    {
        private readonly AppDbContext _context;

        public PartnersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Partners
        public async Task<IActionResult> Index()
        {
            var partners = await _context.Partners
                .Where(p => p.Status == "Active")
                .ToListAsync();

            return View(partners);
        }
    }
}