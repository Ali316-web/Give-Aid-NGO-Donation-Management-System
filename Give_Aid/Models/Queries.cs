using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class Queries
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        // Widened from 20 -> 50: options like "Registration / Account" and
        // "Programme Participation" are longer than 20 characters and would
        // have been truncated or rejected by the old column.
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        // The actual query body. Missing from the original model even though
        // both views already reference it.
        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        // Pending -> In Progress -> Replied -> Closed
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        // The admin's response, shown back to the user who submitted the query.
        public string? Reply { get; set; }

        public DateTime? RepliedDate { get; set; }

        // Links this query back to the ASP.NET Core Identity user who submitted
        // it, so we can show "My Query History" and check who is allowed to view it.
        [StringLength(450)]
        public string? UserId { get; set; }
    }
}