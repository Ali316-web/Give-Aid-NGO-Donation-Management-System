using Give_Aid.Areas.Identity.Data;
using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    public class QueryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QueryController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================================================
        // ADMIN
        // =========================================================

        // GET: Query  (all queries submitted site-wide, admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string? status)
        {
            var queries = _context.Queries.AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                queries = queries.Where(q => q.Status == status);
            }

            var result = await queries
                .OrderByDescending(q => q.SubmittedDate)
                .ToListAsync();

            ViewBag.CurrentStatus = string.IsNullOrEmpty(status) ? "All" : status;
            return View(result);
        }

        // POST: Query/Reply/5  (admin only — writes the response and updates status)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string reply, string status)
        {
            var query = await _context.Queries.FindAsync(id);
            if (query == null)
            {
                return NotFound();
            }

            query.Reply = reply;
            query.Status = string.IsNullOrWhiteSpace(status) ? "Replied" : status;
            query.RepliedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Query/Delete/5  (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries.FirstOrDefaultAsync(q => q.Id == id);
            if (query == null)
            {
                return NotFound();
            }

            return View(query);
        }

        // POST: Query/Delete/5  (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var query = await _context.Queries.FindAsync(id);
            if (query != null)
            {
                _context.Queries.Remove(query);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // SHARED — an admin can view any query; a regular user can
        // only view a query they submitted themselves.
        // =========================================================

        // GET: Query/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries.FirstOrDefaultAsync(q => q.Id == id);
            if (query == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (query.UserId != currentUserId)
                {
                    return Forbid();
                }
            }

            return View(query);
        }

        // =========================================================
        // PUBLIC — submit a query and see your own history
        // =========================================================

        // GET: Query/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(User);

            var vm = new QueryFormViewModel
            {
                // Registration only collects Email/Password, so there's no
                // separate name field on ApplicationUser to prefill from.
                Name = user?.Email ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                Phone = user?.PhoneNumber,
                MyQueries = await _context.Queries
                    .Where(q => q.UserId == userId)
                    .OrderByDescending(q => q.SubmittedDate)
                    .ToListAsync()
            };

            return View(vm);
        }

        // POST: Query/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QueryFormViewModel form)
        {
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                form.MyQueries = await _context.Queries
                    .Where(q => q.UserId == userId)
                    .OrderByDescending(q => q.SubmittedDate)
                    .ToListAsync();

                return View(form);
            }

            _context.Queries.Add(new Queries
            {
                Name = form.Name,
                Email = form.Email,
                Phone = form.Phone,
                Category = form.Category,
                Subject = form.Subject,
                Message = form.Message,
                UserId = userId,
                SubmittedDate = DateTime.Now,
                Status = "Pending"
            });

            await _context.SaveChangesAsync();

            TempData["Message"] = "Your query has been submitted. Our team replies within 48 hours.";
            return RedirectToAction(nameof(Create));
        }
    }
}