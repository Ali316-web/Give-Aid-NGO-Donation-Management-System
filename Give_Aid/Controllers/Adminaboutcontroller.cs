using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiveAid.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAboutController : Controller
    {
        private readonly AppDbContext _context;

        public AdminAboutController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminAbout
        public async Task<IActionResult> Index()
        {
            var vm = new AboutPageViewModel
            {
                Sections = await _context.AboutSections.OrderBy(s => s.Id).ToListAsync(),
                TeamMembers = await _context.TeamMembers.OrderBy(t => t.DisplayOrder).ToListAsync()
            };

            return View(vm);
        }

        // POST: AdminAbout/SaveSection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSection(int id, string title, string content, bool isVisible)
        {
            var section = await _context.AboutSections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            section.Title = title;
            section.Content = content;
            section.IsVisible = isVisible;
            section.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Message"] = $"\"{section.Title}\" saved.";
            return RedirectToAction(nameof(Index));
        }

        // POST: AdminAbout/AddTeamMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeamMember(string name, string role, string? photoUrl)
        {
            var nextOrder = await _context.TeamMembers.AnyAsync()
                ? await _context.TeamMembers.MaxAsync(t => t.DisplayOrder) + 1
                : 0;

            _context.TeamMembers.Add(new TeamMember
            {
                Name = name,
                Role = role,
                PhotoUrl = string.IsNullOrWhiteSpace(photoUrl) ? "https://i.pravatar.cc/160" : photoUrl,
                DisplayOrder = nextOrder
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: AdminAbout/EditTeamMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeamMember(int id, string name, string role)
        {
            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            member.Name = name;
            member.Role = role;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: AdminAbout/DeleteTeamMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeamMember(int id)
        {
            var member = await _context.TeamMembers.FindAsync(id);
            if (member != null)
            {
                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}