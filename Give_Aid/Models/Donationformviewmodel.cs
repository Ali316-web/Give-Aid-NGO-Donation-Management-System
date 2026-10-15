using System.ComponentModel.DataAnnotations;

namespace Give_Aid.Models
{
    public class DonationFormViewModel
    {
        [Required(ErrorMessage = "Please select a cause")]
        public int CauseId { get; set; }

        [Required]
        [Range(100, 100000000, ErrorMessage = "Minimum donation is Rs 100")]
        public decimal Amount { get; set; } = 1000;

        [Required(ErrorMessage = "Card holder name is required")]
        [StringLength(100)]
        [Display(Name = "Card Holder Name")]
        public string CardHolderName { get; set; } = string.Empty;

        // Used only to derive CardLast4 — never saved to the database.
        // This is a demo payment form (no real gateway is called), so we
        // deliberately don't require a mathematically valid card number via
        // [CreditCard] — that check uses the real Luhn algorithm, which
        // rejects the made-up numbers anyone testing this form would type.
        // A simple digit-count check is enough here.
        [Required(ErrorMessage = "Enter a card number")]
        [RegularExpression(@"^[\d\s]{13,19}$", ErrorMessage = "Enter a 13 to 19 digit card number")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter expiry date")]
        [StringLength(7)]
        [Display(Name = "Expiry Date")]
        public string CardExpiry { get; set; } = string.Empty;

        // Captured for form validation only — deliberately never stored anywhere.
        [Required(ErrorMessage = "Enter a valid CVV")]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a card type")]
        [Display(Name = "Card Type")]
        public string CardType { get; set; } = string.Empty;

        // Populated by the controller for the cause-picker pills; not bound from the form.
        public List<DonationCause> Causes { get; set; } = new();
    }
}