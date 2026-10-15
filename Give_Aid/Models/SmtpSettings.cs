namespace Give_Aid.Models
{
    public class SmtpSettings
    {
        public string host { get; set; }
        public int port { get; set; }
        public bool EnableSSL { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
