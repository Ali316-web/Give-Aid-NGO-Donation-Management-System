using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DonationCausesController : Controller
    {
        private readonly AppDbContext _context;

        public DonationCausesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DonationCauses
        public async Task<IActionResult> Index()
        {
            return View(await _context.DonationCauses.ToListAsync());
        }

        // GET: DonationCauses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonationCauses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Cause,Icon,Description,Goal,Status")] DonationCause CauseName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(CauseName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(CauseName);
        }

        // GET: DonationCauses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var CauseName = await _context.DonationCauses.FindAsync(id);
            if (CauseName == null)
            {
                return NotFound();
            }
            return View(CauseName);
        }

        // POST: DonationCauses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            // FIX: this said "Name" instead of "Cause" — since [Bind] is an
            // allowlist, that one-word typo silently blocked the Cause
            // property from ever being bound, no matter what was submitted.
            [Bind("Id,Cause,Icon,Description,Goal,Raised,Status")] DonationCause CauseName)
        {
            // Trust the route's id rather than cross-checking it against the
            // posted form value — a mismatch here used to return NotFound()
            // silently if the hidden Id field ever failed to bind, which is
            // exactly the kind of bug that's invisible until it happens.
            CauseName.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(CauseName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CauseNameExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(CauseName);
        }

        // POST: DonationCauses/ToggleStatus/5 — the power-off icon in the mockup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var CauseName = await _context.DonationCauses.FindAsync(id);
            if (CauseName == null)
            {
                return NotFound();
            }

            CauseName.Status = CauseName.Status == "Active" ? "Inactive" : "Active";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: DonationCauses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var CauseName = await _context.DonationCauses.FirstOrDefaultAsync(m => m.Id == id);
            if (CauseName == null)
            {
                return NotFound();
            }

            return View(CauseName);
        }

        // POST: DonationCauses/Delete/5
        // Donations already linked to this cause keep their historical
        // record — the FK on Donation was created with ReferentialAction.Cascade
        // originally, so double check that before relying on this message
        // being accurate; if it's still Cascade, deleting a cause would also
        // delete its donations, which contradicts what the mockup promises.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var CauseName = await _context.DonationCauses.FindAsync(id);
            if (CauseName != null)
            {
                _context.DonationCauses.Remove(CauseName);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CauseNameExists(int id)
        {
            return _context.DonationCauses.Any(e => e.Id == id);
        }
    }
}