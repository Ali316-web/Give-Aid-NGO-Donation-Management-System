using Give_Aid.Models;
using Microsoft.AspNetCore.Mvc;

namespace Give_Aid.Controllers
{
    // Public-facing Contact Us page. Anonymous access is intentional here —
    // unlike Queries, contact.html isn't a member-only feature and has no
    // "your message history" section to tie back to a logged-in user.
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Contact
        public IActionResult Index()
        {
            return View(new ContactFormViewModel());
        }

        // POST: Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactFormViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            _context.AdminContacts.Add(new AdminContact
            {
                FullName = form.FullName,
                FromEmail = form.Email,
                Subject = form.Subject,
                Message = form.Message,
                Date = DateTime.Now,
                Status = "Unread"
            });

            await _context.SaveChangesAsync();

            TempData["Message"] = "Thanks for reaching out — our team will get back to you soon.";
            return RedirectToAction(nameof(Index));
        }
    }
}