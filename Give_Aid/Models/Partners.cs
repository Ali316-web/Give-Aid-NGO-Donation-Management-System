using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class Partners
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string OrganizationName { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string OrganizationType { get; set; } = string.Empty;

        // All four fields below existed in the admin-partners.html form and
        // the public partners.html cards, but had nothing backing them at all.
        [StringLength(300)]
        public string? Website { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? ContactEmail { get; set; }

        // A pasted URL rather than a real file upload — see the note below
        // the file list if you want actual logo file uploads instead.
        [StringLength(300)]
        public string? LogoUrl { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Active";
    }
}