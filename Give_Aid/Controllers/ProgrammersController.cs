using Give_Aid.Areas.Identity.Data;
using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    // No class-level [Authorize]: Index, Details and Interested are public
    // (or login-gated individually where it matters). Every admin-only
    // action below carries its own [Authorize(Roles = "Admin")].
    public class ProgrammersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProgrammersController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================================================
        // PUBLIC
        // =========================================================

        // GET: Programmers (public)
        public async Task<IActionResult> Index()
        {
            var programmes = await _context.Programmers
                .OrderBy(p => p.ProgrammeDate)
                .ToListAsync();

            return View(programmes);
        }

        // GET: Programmers/Details/5 (public)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programme = await _context.Programmers.FirstOrDefaultAsync(m => m.Id == id);
            if (programme == null)
            {
                return NotFound();
            }

            ViewBag.ParticipantCount = await _context.ProgrammeParticipants
                .CountAsync(p => p.ProgrammeId == id);

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);
                ViewBag.AlreadyInterested = await _context.ProgrammeParticipants
                    .AnyAsync(p => p.ProgrammeId == id && p.UserId == userId);
            }
            else
            {
                ViewBag.AlreadyInterested = false;
            }

            return View(programme);
        }

        // POST: Programmers/Interested/5 (must be logged in)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Interested(int id, string? notes)
        {
            var programme = await _context.Programmers.FindAsync(id);
            if (programme == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var alreadyRegistered = await _context.ProgrammeParticipants
                .AnyAsync(p => p.ProgrammeId == id && p.UserId == user.Id);

            if (!alreadyRegistered)
            {
                _context.ProgrammeParticipants.Add(new ProgrammeParticipant
                {
                    ProgrammeId = id,
                    Name = user.UserName ?? user.Email ?? "Member",
                    Email = user.Email ?? string.Empty,
                    Notes = notes,
                    Status = "Interested",
                    RegisteredDate = DateTime.Now,
                    UserId = user.Id
                });

                await _context.SaveChangesAsync();
                TempData["Message"] = "Thanks — you're registered! We'll notify the organizing NGO.";
            }
            else
            {
                TempData["Message"] = "You're already registered for this programme.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // =========================================================
        // ADMIN
        // =========================================================

        // GET: Programmers/AdminIndex
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var programmes = await _context.Programmers
                .OrderBy(p => p.ProgrammeDate)
                .ToListAsync();

            // Live participant counts, keyed by programme id, so the admin
            // table shows real registrations instead of the stale int column.
            var counts = await _context.ProgrammeParticipants
                .GroupBy(p => p.ProgrammeId)
                .Select(g => new { ProgrammeId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ProgrammeId, x => x.Count);

            ViewBag.ParticipantCounts = counts;
            return View(programmes);
        }

        // GET: Programmers/AdminDetails/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programme = await _context.Programmers.FirstOrDefaultAsync(m => m.Id == id);
            if (programme == null)
            {
                return NotFound();
            }

            ViewBag.ParticipantCount = await _context.ProgrammeParticipants
                .CountAsync(p => p.ProgrammeId == id);

            return View(programme);
        }

        // GET: Programmers/Participants/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Participants(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programme = await _context.Programmers.FindAsync(id);
            if (programme == null)
            {
                return NotFound();
            }

            var participants = await _context.ProgrammeParticipants
                .Where(p => p.ProgrammeId == id)
                .OrderByDescending(p => p.RegisteredDate)
                .ToListAsync();

            ViewBag.Programme = programme;
            return View(participants);
        }

        // GET: Programmers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Programmers/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Programme,Category,Location,ProgrammeDate,Status,Description,ImageUrl")] Programmers model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(model);
        }

        // GET: Programmers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Programmers.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Programmers/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Programme,Category,Location,ProgrammeDate,Status,Description,ImageUrl")] Programmers model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgrammersExists(model.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(model);
        }

        // GET: Programmers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programme = await _context.Programmers.FirstOrDefaultAsync(m => m.Id == id);
            if (programme == null)
            {
                return NotFound();
            }

            return View(programme);
        }

        // POST: Programmers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programme = await _context.Programmers.FindAsync(id);
            if (programme != null)
            {
                // Registered interest records are removed alongside the
                // programme, matching admin-programmes.html's delete warning.
                var participants = _context.ProgrammeParticipants.Where(p => p.ProgrammeId == id);
                _context.ProgrammeParticipants.RemoveRange(participants);

                _context.Programmers.Remove(programme);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex));
        }

        private bool ProgrammersExists(int id)
        {
            return _context.Programmers.Any(e => e.Id == id);
        }
    }
}