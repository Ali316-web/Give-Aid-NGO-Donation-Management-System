namespace Give_Aid.Models
{
    public class AdminUserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; } = new();

        public bool IsAdmin => Roles.Contains("Admin");
    }
}