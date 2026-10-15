using Give_Aid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Give_Aid.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeIndexViewModel
            {
                Causes = await _context.DonationCauses
                    .Where(c => c.Status == "Active")
                    .ToListAsync(),

                Programmes = await _context.Programmers
                    .Where(p => p.Status == "Upcoming")
                    .OrderBy(p => p.ProgrammeDate)
                    .Take(3)
                    .ToListAsync(),

                Partners = await _context.Partners
                    .Where(p => p.Status == "Active")
                    .ToListAsync()
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }



        // Replace the existing "public IActionResult Admin()" method in
        // HomeController.cs with this one. Requires the AppDbContext field/constructor
        // injection already added for Index() — no new dependencies needed.

        public async Task<IActionResult> Admin()
        {
            var now = DateTime.Now;
            var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfThisMonth.AddMonths(-1);
            var sixMonthsAgo = startOfThisMonth.AddMonths(-5);
            var quarterStart = startOfThisMonth.AddMonths(-2);

            var vm = new AdminDashboardViewModel();

            // ---------- Top KPI row ----------

            vm.TotalMembers = await _context.Members.CountAsync();
            var membersThisMonth = await _context.Members.CountAsync(m => m.JoinedDate >= startOfThisMonth);
            var membersLastMonth = await _context.Members.CountAsync(m => m.JoinedDate >= startOfLastMonth && m.JoinedDate < startOfThisMonth);
            vm.MembersDeltaPercent = PercentChange(membersLastMonth, membersThisMonth);

            vm.TotalDonationsCount = await _context.Donations.CountAsync();
            var donationsThisMonth = await _context.Donations.CountAsync(d => d.DonationDate >= startOfThisMonth);
            var donationsLastMonth = await _context.Donations.CountAsync(d => d.DonationDate >= startOfLastMonth && d.DonationDate < startOfThisMonth);
            vm.DonationsCountDeltaPercent = PercentChange(donationsLastMonth, donationsThisMonth);

            vm.TotalDonationAmount = await _context.Donations
                .SumAsync(d => (decimal?)d.Amount) ?? 0m;

            var amountThisMonth = await _context.Donations
                .Where(d => d.DonationDate >= startOfThisMonth)
                .SumAsync(d => (decimal?)d.Amount) ?? 0m;
            var amountLastMonth = await _context.Donations
                .Where(d => d.DonationDate >= startOfLastMonth && d.DonationDate < startOfThisMonth)
                .SumAsync(d => (decimal?)d.Amount) ?? 0m;
            vm.DonationAmountDeltaPercent = PercentChange((double)amountLastMonth, (double)amountThisMonth);

            vm.PendingQueries = await _context.Queries.CountAsync(q => q.Status == "Pending");

            // ---------- Second KPI row (no deltas — no historical snapshot to compare against) ----------

            vm.TotalNGOs = await _context.AssociatedNGOs.CountAsync();
            vm.TotalPartners = await _context.Partners.CountAsync();
            vm.TotalProgrammes = await _context.Programmers.CountAsync();
            vm.UpcomingProgrammes = await _context.Programmers.CountAsync(p => p.Status == "Upcoming");

            // ---------- Donations by month (last 6 months, in Rs millions) ----------

            var donationsRaw = await _context.Donations
                .Where(d => d.DonationDate >= sixMonthsAgo)
                .Select(d => new { d.DonationDate, d.Amount })
                .ToListAsync();

            vm.DonationsByMonth = Enumerable.Range(0, 6)
                .Select(i => startOfThisMonth.AddMonths(-5 + i))
                .Select(monthStart => new ChartPoint
                {
                    Label = monthStart.ToString("MMM"),
                    Value = (double)donationsRaw
                        .Where(d => d.DonationDate.Year == monthStart.Year && d.DonationDate.Month == monthStart.Month)
                        .Sum(d => d.Amount) / 1_000_000d
                })
                .ToList();

            // ---------- Donations by cause (this quarter, top 4 + Other) ----------

            var causeDonations = await _context.Donations
                .Where(d => d.DonationDate >= quarterStart)
                .Include(d => d.DonationCause)
                .Select(d => new { Cause = d.DonationCause != null ? d.DonationCause.Cause : "Other", d.Amount })
                .ToListAsync();

            var grouped = causeDonations
                .GroupBy(d => d.Cause)
                .Select(g => new { Cause = g.Key, Total = g.Sum(x => x.Amount) })
                .OrderByDescending(g => g.Total)
                .ToList();

            var totalQuarter = grouped.Sum(g => (double)g.Total);
            var palette = new[] { "#1F5D50", "#F0A93C", "#E2694B", "#2A6FA8", "#9CA5A2" };

            var topCauses = grouped.Take(4).ToList();
            var otherTotal = grouped.Skip(4).Sum(g => (double)g.Total);

            vm.DonationsByCause = topCauses
                .Select((g, i) => new ChartPoint
                {
                    Label = g.Cause,
                    Value = totalQuarter > 0 ? Math.Round((double)g.Total / totalQuarter * 100) : 0,
                    Color = palette[i]
                })
                .ToList();

            if (otherTotal > 0 && totalQuarter > 0)
            {
                vm.DonationsByCause.Add(new ChartPoint
                {
                    Label = "Other",
                    Value = Math.Round(otherTotal / totalQuarter * 100),
                    Color = palette[4]
                });
            }

            // ---------- Programme participation (top 4 by interested count) ----------

            var participationCounts = await _context.ProgrammeParticipants
                .GroupBy(p => p.ProgrammeId)
                .Select(g => new { ProgrammeId = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(4)
                .ToListAsync();

            var programmeIds = participationCounts.Select(p => p.ProgrammeId).ToList();
            var programmeNames = await _context.Programmers
                .Where(p => programmeIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Programme);

            vm.ProgrammeParticipation = participationCounts
                .Select(p => new ChartPoint
                {
                    Label = programmeNames.TryGetValue(p.ProgrammeId, out var name)
                        ? (name.Length > 10 ? name.Substring(0, 10) : name)
                        : "Unknown",
                    Value = p.Count,
                    Color = "#1F5D50"
                })
                .ToList();

            // ---------- Member registration by month (last 6 months) ----------

            var membersRaw = await _context.Members
                .Where(m => m.JoinedDate >= sixMonthsAgo)
                .Select(m => m.JoinedDate)
                .ToListAsync();

            vm.MemberRegistrationByMonth = Enumerable.Range(0, 6)
                .Select(i => startOfThisMonth.AddMonths(-5 + i))
                .Select(monthStart => new ChartPoint
                {
                    Label = monthStart.ToString("MMM"),
                    Value = membersRaw.Count(d => d.Year == monthStart.Year && d.Month == monthStart.Month),
                    Color = "#F0A93C"
                })
                .ToList();

            // ---------- Recent activity feed (merged from 4 sources, newest 5) ----------

            var activity = new List<RecentActivityItem>();

            var recentDonations = await _context.Donations.OrderByDescending(d => d.DonationDate).Take(5).ToListAsync();
            activity.AddRange(recentDonations.Select(d => new RecentActivityItem
            {
                Icon = "fa-solid fa-hand-holding-dollar",
                Text = $"New donation — Rs {d.Amount:N0}",
                User = d.DonorName,
                Date = d.DonationDate,
                BadgeClass = d.PaymentStatus switch { "Completed" => "badge-success", "Pending" => "badge-warning", _ => "badge-danger" },
                BadgeText = d.PaymentStatus
            }));

            var recentMembers = await _context.Members.OrderByDescending(m => m.JoinedDate).Take(5).ToListAsync();
            activity.AddRange(recentMembers.Select(m => new RecentActivityItem
            {
                Icon = "fa-solid fa-user-plus",
                Text = "New member registered",
                User = m.FullName,
                Date = m.JoinedDate,
                BadgeClass = "badge-success",
                BadgeText = m.Status
            }));

            var recentQueries = await _context.Queries.OrderByDescending(q => q.SubmittedDate).Take(5).ToListAsync();
            activity.AddRange(recentQueries.Select(q => new RecentActivityItem
            {
                Icon = "fa-solid fa-comments",
                Text = "New query submitted",
                User = q.Name,
                Date = q.SubmittedDate,
                BadgeClass = q.Status switch { "Pending" => "badge-warning", "Replied" => "badge-success", "Closed" => "badge-neutral", _ => "badge-info" },
                BadgeText = q.Status
            }));

            var recentParticipants = await _context.ProgrammeParticipants
                .OrderByDescending(p => p.RegisteredDate)
                .Take(5)
                .ToListAsync();
            activity.AddRange(recentParticipants.Select(p => new RecentActivityItem
            {
                Icon = "fa-solid fa-calendar-plus",
                Text = "Programme interest registered",
                User = p.Name,
                Date = p.RegisteredDate,
                BadgeClass = "badge-info",
                BadgeText = p.Status
            }));

            vm.RecentActivity = activity.OrderByDescending(a => a.Date).Take(5).ToList();

            return View(vm);
        }

        private static double PercentChange(double previous, double current)
        {
            if (previous == 0)
            {
                return current > 0 ? 100 : 0;
            }
            return Math.Round((current - previous) / previous * 100, 1);
        }


        // NOTE: these three stub actions (Partners, NGOs, gallery) look like
        // leftovers from before the dedicated PartnersController,
        // AssociatedNGOsController, and GalleriesController existed. If
        // nothing still links to Home/Partners, Home/NGOs, or Home/gallery,
        // consider deleting these along with their unused views — otherwise
        // you now have two different pages answering for the same content
        // (e.g. /Home/Partners showing a static view vs /Partners showing
        // live data), which is exactly the kind of duplicate-route confusion
        // that caused the Partners mixup earlier in this project.


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}