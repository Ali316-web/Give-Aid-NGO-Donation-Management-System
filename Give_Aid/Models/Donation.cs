using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Give_Aid.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DonorName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string DonorEmail { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public int CauseId { get; set; }

        [ForeignKey("CauseId")]
        public DonationCause? DonationCause { get; set; }

        [Required]
        [StringLength(100)]
        public string CardHolderName { get; set; } = string.Empty;

        // Never store the full card number or CVV. This keeps only the last
        // 4 digits, purely for display (e.g. "Visa •• 4471"), matching what
        // admin-donations.html actually shows — it never displayed a full
        // card number either.
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string CardLast4 { get; set; } = string.Empty;

        // "Visa", "Mastercard", "UnionPay" — from the Card Type dropdown.
        [Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string TransactionId { get; set; } = string.Empty;

        public DateTime DonationDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = "Pending";

        // Links to the Identity user who made the donation, if they were
        // logged in. Null for anonymous/guest donations.
        [StringLength(450)]
        public string? UserId { get; set; }
    }
}