using Give_Aid.Areas.Identity.Data;
using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Give_Aid.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminUsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: AdminUsers
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var list = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }

            return View(list.OrderBy(u => u.Email).ToList());
        }

        // POST: AdminUsers/ToggleAdmin/{id}
        // Adds or removes the Admin role. Refuses to let an admin remove
        // their own Admin role, since that would lock them out with no
        // recovery path other than editing the database directly.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                var currentUserId = _userManager.GetUserId(User);
                if (user.Id == currentUserId)
                {
                    TempData["Message"] = "You can't remove your own Admin role.";
                    return RedirectToAction(nameof(Index));
                }

                await _userManager.RemoveFromRoleAsync(user, "Admin");
                TempData["Message"] = $"{user.Email} is no longer an Admin.";
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["Message"] = $"{user.Email} is now an Admin.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminUsers/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            return View(new AdminUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            });
        }

        // POST: AdminUsers/Delete/{id}
        // Refuses to let an admin delete their own account for the same
        // self-lockout reason as ToggleAdmin above.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                TempData["Message"] = "You can't delete your own account while logged in as it.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(user);
            TempData["Message"] = $"{user.Email} was deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}