using Give_Aid.Areas.Identity.Data;
using Give_Aid.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AboutSection>().HasData(
            new AboutSection
            {
                Id = 1,
                SectionKey = "what-we-do",
                Title = "What We Do",
                Content = "<h2>Connecting resources to where they matter most</h2><p>We vet and onboard associated NGOs, run transparent donation pipelines by cause, and organize joint programmes across healthcare, education and welfare — so donors can trust exactly where their contribution lands.</p>",
                IsVisible = true,
                UpdatedDate = new DateTime(2026, 1, 1)   // fixed value — do not use DateTime.Now here
            },
            new AboutSection
            {
                Id = 2,
                SectionKey = "mission",
                Title = "Our Mission",
                Content = "<h2>Shelter, dignity and opportunity for every community we reach</h2><p>We believe lasting change grows from local roots. Our mission is to strengthen grassroots NGOs with funding, visibility and shared infrastructure — not to replace them.</p>",
                IsVisible = true,
                UpdatedDate = new DateTime(2026, 1, 1)
            }
        );
    }

    public DbSet<Donation> Donations { get; set; }
    public DbSet<DonationCause> DonationCauses { get; set; }
    public DbSet<Members> Members { get; internal set; }
    public DbSet<Partners> Partners { get; internal set; }
    public DbSet<Programmers> Programmers { get; internal set; }
    public DbSet<Gallery> Gallery { get; internal set; }
    public DbSet<Queries> Queries { get; internal set; }
    public DbSet<AdminContact> AdminContacts { get; internal set; }
    public DbSet<AssociatedNGOs> AssociatedNGOs { get; internal set; }
    public DbSet<AboutSection> AboutSections { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }  
    public DbSet<ProgrammeParticipant> ProgrammeParticipants { get; set;}


}