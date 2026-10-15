using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class AdminContact
    {
        [Key]
        public int Id { get; set; }

        // Missing from the original model even though contact.html's form
        // collects "Full Name" and admin-messages.html displays it under "From".
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string FromEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        // Unread -> Read -> Replied
        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Unread";

        // The admin's response, shown in admin-messages.html's reply box.
        // Nothing in the original model backed this field at all.
        [StringLength(2000)]
        public string? Reply { get; set; }

        public DateTime? RepliedDate { get; set; }
    }
}
