using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Give_Aid.Models
{
    public class Gallery
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        // Missing before, even though both the admin form and the public
        // filter chips (Healthcare / Education / etc.) need it to work.
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        // Optional link to a specific Programme ("Linked Programme" in the
        // admin form). Nullable — most images won't be tied to one.
        public int? ProgrammeId { get; set; }

        [ForeignKey("ProgrammeId")]
        public Programmers? Programme { get; set; }
    }
}