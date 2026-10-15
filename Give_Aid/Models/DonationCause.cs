using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Give_Aid.Models
{
    public class DonationCause
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Cause { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Raised { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Active";

        // All three below existed in admin-causes.html's "Add Donation Cause"
        // form but had nothing backing them at all.

        // A Font Awesome icon class, e.g. "fa-solid fa-child-reaching".
        [StringLength(60)]
        public string? Icon { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Goal { get; set; }
    }
}