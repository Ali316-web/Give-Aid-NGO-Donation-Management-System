using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class Members
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime JoinedDate { get; set; } = DateTime.Now;

        // Links this Member record back to the ASP.NET Core Identity user
        // (IdentityUser.Id, which is a string). Used to check whether a
        // Member row already exists for a given logged-in user.
        [StringLength(450)]
        public string? UserId { get; set; }


    }
}