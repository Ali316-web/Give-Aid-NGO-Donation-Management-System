using Give_Aid.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Give_Aid.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<SmtpSettings> smtpSettings, ILogger<EmailSender> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var client = new SmtpClient(_smtpSettings.host, _smtpSettings.port)
            {
                EnableSsl = _smtpSettings.EnableSSL,
                Credentials = new NetworkCredential(
                    _smtpSettings.username?.Trim(),
                    _smtpSettings.password?.Replace(" ", "").Trim())
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent to {Email} via {Host}:{Port}", email, _smtpSettings.host, _smtpSettings.port);
            }
            catch (Exception ex)
            {
                // Log the real reason instead of letting the caller (Register/ForgotPassword pages)
                // fail silently or crash the request with an unhandled SmtpException.
                _logger.LogError(ex, "Failed to send email to {Email} via {Host}:{Port}", email, _smtpSettings.host, _smtpSettings.port);
                throw;
            }
        }
    }
}