using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class Programmers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Programme { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime ProgrammeDate { get; set; }

        // Kept for backward compatibility, but no longer the source of truth
        // for participant counts — the admin views now count real rows in
        // ProgrammeParticipants instead. Left here rather than dropped to
        // avoid an unnecessary destructive column change.
        public int Participants { get; set; } = 0;

        public string Status { get; set; } = "Upcoming";

        // Both missing before, even though admin-programmes.html's form and
        // programme-details.html's page both need them.
        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(300)]
        public string? ImageUrl { get; set; }
    }
}