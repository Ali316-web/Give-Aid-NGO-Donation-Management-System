namespace Give_Aid.Models
{
    public class HomeIndexViewModel
    {
        public List<DonationCause> Causes { get; set; } = new();
        public List<Programmers> Programmes { get; set; } = new();
        public List<Partners> Partners { get; set; } = new();
    }
}