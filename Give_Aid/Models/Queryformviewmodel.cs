using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class QueryFormViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; } = string.Empty;

        // Not bound from the form — populated by the controller so the same
        // view can show the submit form and the user's own query history together.
        public List<Queries> MyQueries { get; set; } = new();
    }
}