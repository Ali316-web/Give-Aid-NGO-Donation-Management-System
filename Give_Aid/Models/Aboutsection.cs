using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class AboutSection
    {
        [Key]
        public int Id { get; set; }

        // Stable key used to find this section from views/code, e.g. "what-we-do", "mission".
        // Not shown in the admin UI, just used to match a DB row to a spot on the public page.
        [Required]
        [StringLength(50)]
        public string SectionKey { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        // Rich HTML captured from the admin's contenteditable editor.
        // Rendered on the public page with @Html.Raw, so only trusted admins should be able to edit this.
        [Required]
        public string Content { get; set; } = string.Empty;

        public bool IsVisible { get; set; } = true;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}