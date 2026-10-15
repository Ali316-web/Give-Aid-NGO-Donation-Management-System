namespace Give_Aid.Models
{
    public class AboutPageViewModel
    {
        public List<AboutSection> Sections { get; set; } = new();
        public List<TeamMember> TeamMembers { get; set; } = new();
    }
}