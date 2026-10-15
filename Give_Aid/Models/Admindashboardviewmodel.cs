namespace Give_Aid.Models
{
    public class ChartPoint
    {
        public string Label { get; set; } = string.Empty;
        public double Value { get; set; }
        public string? Color { get; set; }
    }

    public class RecentActivityItem
    {
        public string Icon { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string BadgeClass { get; set; } = "badge-neutral";
        public string BadgeText { get; set; } = string.Empty;
    }

    public class AdminDashboardViewModel
    {
        public int TotalMembers { get; set; }
        public double MembersDeltaPercent { get; set; }

        public int TotalDonationsCount { get; set; }
        public double DonationsCountDeltaPercent { get; set; }

        public decimal TotalDonationAmount { get; set; }
        public double DonationAmountDeltaPercent { get; set; }

        public int PendingQueries { get; set; }

        public int TotalNGOs { get; set; }
        public int TotalPartners { get; set; }
        public int TotalProgrammes { get; set; }
        public int UpcomingProgrammes { get; set; }

        public List<ChartPoint> DonationsByMonth { get; set; } = new();
        public List<ChartPoint> DonationsByCause { get; set; } = new();
        public List<ChartPoint> ProgrammeParticipation { get; set; } = new();
        public List<ChartPoint> MemberRegistrationByMonth { get; set; } = new();

        public List<RecentActivityItem> RecentActivity { get; set; } = new();
    }
}