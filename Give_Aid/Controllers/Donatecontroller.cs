using Give_Aid.Areas.Identity.Data;
using Give_Aid.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Give_Aid.Controllers
{
    // Public-facing Donate flow. Deliberately no [Authorize] — donations can
    // be made by anonymous visitors, matching donate.html (which still shows
    // Login/Register in the header rather than requiring an account).
    // If a visitor happens to be logged in, we quietly link the donation to
    // their account via UserId so it can show up in a "My Donations" page later.
    public class DonateController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonateController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Donate
        public async Task<IActionResult> Index()
        {
            var vm = new DonationFormViewModel
            {
                Causes = await _context.DonationCauses.ToListAsync()
            };

            return View(vm);
        }

        // POST: Donate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DonationFormViewModel form)
        {
            if (!ModelState.IsValid)
            {
                form.Causes = await _context.DonationCauses.ToListAsync();
                return View(form);
            }

            // This is a demo payment form (as donate.html says outright) — no
            // real payment gateway is called. We simulate an always-successful
            // charge and never store the full card number or CVV anywhere.
            var digitsOnly = new string(form.CardNumber.Where(char.IsDigit).ToArray());
            var last4 = digitsOnly.Length >= 4 ? digitsOnly[^4..] : digitsOnly.PadLeft(4, '0');

            string? userId = null;
            string donorName;
            string donorEmail;

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                userId = user?.Id;
                donorEmail = user?.Email ?? string.Empty;
                donorName = user?.UserName ?? donorEmail;
            }
            else
            {
                // No name/email fields exist on this specific form in
                // donate.html, so guest donations fall back to a generic
                // label. See the note below the file list for how to add
                // proper guest fields if you want named anonymous receipts.
                donorName = "Guest Donor";
                donorEmail = "guest@giveaid.org";
            }

            var donation = new Donation
            {
                DonorName = donorName,
                DonorEmail = donorEmail,
                Amount = form.Amount,
                CauseId = form.CauseId,
                CardHolderName = form.CardHolderName,
                CardLast4 = last4,
                PaymentMethod = form.CardType,
                TransactionId = "TXN-" + Random.Shared.Next(10000000, 99999999),
                DonationDate = DateTime.Now,
                PaymentStatus = "Completed",
                UserId = userId
            };

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            // Carry the new donation's id to the confirmation page via TempData
            // rather than the URL, so it isn't sitting in the address bar/history.
            TempData["DonationId"] = donation.Id;
            return RedirectToAction(nameof(Confirmation));
        }

        // GET: Donate/Confirmation
        public async Task<IActionResult> Confirmation()
        {
            if (TempData["DonationId"] is not int donationId)
            {
                return RedirectToAction(nameof(Index));
            }

            var donation = await _context.Donations
                .Include(d => d.DonationCause)
                .FirstOrDefaultAsync(d => d.Id == donationId);

            if (donation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Keep it available if the page is refreshed once.
            TempData.Keep("DonationId");

            return View(donation);
        }
    }
}