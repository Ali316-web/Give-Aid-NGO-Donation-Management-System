using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    // No class-level [Authorize] here on purpose: Index and Details are the
    // public pages (ngos.html). Every admin-only action below is individually
    // marked [Authorize(Roles = "Admin")] instead.
    public class AssociatedNGOsController : Controller
    {
        private readonly AppDbContext _context;

        public AssociatedNGOsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AssociatedNGOs (public)
        public async Task<IActionResult> Index()
        {
            var ngos = await _context.AssociatedNGOs
                .Where(n => n.Status == "Active")
                .ToListAsync();

            return View(ngos);
        }

        // GET: AssociatedNGOs/AdminIndex (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            return View(await _context.AssociatedNGOs.ToListAsync());
        }

        // GET: AssociatedNGOs/Details/5 (public)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngo = await _context.AssociatedNGOs.FirstOrDefaultAsync(m => m.Id == id);
            if (ngo == null)
            {
                return NotFound();
            }

            return View(ngo);
        }

        // GET: AssociatedNGOs/AdminDetails/5 (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngo = await _context.AssociatedNGOs.FirstOrDefaultAsync(m => m.Id == id);
            if (ngo == null)
            {
                return NotFound();
            }

            return View(ngo);
        }

        // GET: AssociatedNGOs/Create (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssociatedNGOs/Create (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,NGOName,Location,FocusArea,Email,Phone,Website,Address,Description,LogoUrl,Status")] AssociatedNGOs ngo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ngo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(ngo);
        }

        // GET: AssociatedNGOs/Edit/5 (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngo = await _context.AssociatedNGOs.FindAsync(id);
            if (ngo == null)
            {
                return NotFound();
            }
            return View(ngo);
        }

        // POST: AssociatedNGOs/Edit/5 (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,NGOName,Location,FocusArea,Email,Phone,Website,Address,Description,LogoUrl,Status")] AssociatedNGOs ngo)
        {
            if (id != ngo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ngo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssociatedNGOsExists(ngo.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(ngo);
        }

        // GET: AssociatedNGOs/Delete/5 (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngo = await _context.AssociatedNGOs.FirstOrDefaultAsync(m => m.Id == id);
            if (ngo == null)
            {
                return NotFound();
            }

            return View(ngo);
        }

        // POST: AssociatedNGOs/Delete/5 (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ngo = await _context.AssociatedNGOs.FindAsync(id);
            if (ngo != null)
            {
                _context.AssociatedNGOs.Remove(ngo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex));
        }

        private bool AssociatedNGOsExists(int id)
        {
            return _context.AssociatedNGOs.Any(e => e.Id == id);
        }
    }
}