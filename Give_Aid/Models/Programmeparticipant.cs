using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Give_Aid.Models
{
    public class ProgrammeParticipant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProgrammeId { get; set; }

        [ForeignKey("ProgrammeId")]
        public Programmers? Programme { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Interested -> Confirmed
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Interested";

        public DateTime RegisteredDate { get; set; } = DateTime.Now;

        // Links to the Identity user who registered, so we can prevent the
        // same person registering twice for the same programme.
        [StringLength(450)]
        public string? UserId { get; set; }
    }
}