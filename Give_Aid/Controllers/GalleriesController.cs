using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    // No class-level [Authorize]: Index is the public gallery.html page.
    // Every admin-only action below carries its own [Authorize(Roles = "Admin")].
    public class GalleriesController : Controller
    {
        private readonly AppDbContext _context;

        public GalleriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Galleries (public)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gallery.ToListAsync());
        }

        // GET: Galleries/AdminIndex (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            return View(await _context.Gallery.Include(g => g.Programme).ToListAsync());
        }

        // GET: Galleries/Create (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Programmes = _context.Programmers.ToList();
            return View();
        }

        // POST: Galleries/Create (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Image,Category,ProgrammeId")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminIndex));
            }

            ViewBag.Programmes = _context.Programmers.ToList();
            return View(gallery);
        }

        // GET: Galleries/Edit/5 (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }

            ViewBag.Programmes = _context.Programmers.ToList();
            return View(gallery);
        }

        // POST: Galleries/Edit/5 (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Image,Category,ProgrammeId")] Gallery gallery)
        {
            if (id != gallery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(AdminIndex));
            }

            ViewBag.Programmes = _context.Programmers.ToList();
            return View(gallery);
        }

        // GET: Galleries/Delete/5 (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery.FirstOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // POST: Galleries/Delete/5 (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery != null)
            {
                _context.Gallery.Remove(gallery);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex));
        }

        private bool GalleryExists(int id)
        {
            return _context.Gallery.Any(e => e.Id == id);
        }
    }
}