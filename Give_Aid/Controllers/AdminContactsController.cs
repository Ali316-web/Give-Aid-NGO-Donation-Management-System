using Give_Aid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminContactsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminContactsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminContacts
        public async Task<IActionResult> Index()
        {
            var contacts = await _context.AdminContacts
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return View(contacts);
        }

        // GET: AdminContacts/Edit/5
        // Opening this now also marks the message as Read, if it was Unread.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.AdminContacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            if (contact.Status == "Unread")
            {
                contact.Status = "Read";
                await _context.SaveChangesAsync();
            }

            return View(contact);
        }

        // POST: AdminContacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,FullName,FromEmail,Subject,Message,Date,Status,Reply")] AdminContact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // If the admin just typed a reply that wasn't there before, stamp
                // the reply time and bump the status, matching admin-messages.html's
                // "Send Reply" button behaviour.
                var existing = await _context.AdminContacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (existing != null && string.IsNullOrWhiteSpace(existing.Reply) && !string.IsNullOrWhiteSpace(contact.Reply))
                {
                    contact.RepliedDate = DateTime.Now;
                    contact.Status = "Replied";
                }
                else
                {
                    contact.RepliedDate = existing?.RepliedDate;
                }

                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(contact);
        }

        // GET: AdminContacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.AdminContacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: AdminContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.AdminContacts.FindAsync(id);
            if (contact != null)
            {
                _context.AdminContacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.AdminContacts.Any(c => c.Id == id);
        }
    }
}