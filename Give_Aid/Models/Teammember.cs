using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Role { get; set; } = string.Empty;

        [StringLength(300)]
        public string? PhotoUrl { get; set; }

        // Controls display order on both the admin table and the public team grid.
        public int DisplayOrder { get; set; } = 0;
    }
}