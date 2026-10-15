using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class AssociatedNGOs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string NGOName { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string FocusArea { get; set; } = string.Empty;

        // All fields below existed in admin-ngos.html's "Add / Edit" form but
        // had nothing backing them at all. Left nullable so adding them via
        // migration doesn't hit a NOT NULL wall against any existing rows.
        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(300)]
        public string? Website { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // A pasted URL rather than a real file upload, same simplification
        // used for Partners' logo and Team Members' photos earlier.
        [StringLength(300)]
        public string? LogoUrl { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Active";
    }
}