using System.Net;
using System.Net.Mail;
using cms_webapi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace cms_webapi.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailService> _logger;

        public MailService(IConfiguration configuration, ILogger<MailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true, string? cc = null, string? bcc = null, List<string>? attachments = null)
        {
            var fromEmail = _configuration["SmtpSettings:FromEmail"];
            var fromName = _configuration["SmtpSettings:FromName"];
            return await SendEmailAsync(to, subject, body, fromEmail, fromName, isHtml, cc, bcc, attachments);
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, string? fromEmail = null, string? fromName = null, bool isHtml = true, string? cc = null, string? bcc = null, List<string>? attachments = null)
        {
            try
            {
                var smtpHost = _configuration["SmtpSettings:Host"];
                var smtpPort = int.Parse(_configuration["SmtpSettings:Port"] ?? "587");
                var enableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"] ?? "true");
                var smtpUsername = _configuration["SmtpSettings:Username"];
                var smtpPassword = _configuration["SmtpSettings:Password"];
                var timeout = int.Parse(_configuration["SmtpSettings:Timeout"] ?? "30");

                var defaultFromEmail = _configuration["SmtpSettings:FromEmail"] ?? "noreply@v3rii.com";
                var defaultFromName = _configuration["SmtpSettings:FromName"] ?? "V3RII CRM System";

                if (string.IsNullOrWhiteSpace(smtpHost))
                {
                    _logger.LogError("SMTP Host is not configured");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(smtpUsername) || string.IsNullOrWhiteSpace(smtpPassword))
                {
                    _logger.LogError("SMTP Username or Password is not configured");
                    return false;
                }

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = enableSsl,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    Timeout = timeout * 1000
                };

                using var message = new MailMessage();

                // From
                var fromAddress = new MailAddress(fromEmail ?? defaultFromEmail, fromName ?? defaultFromName);
                message.From = fromAddress;

                // To
                message.To.Add(to);

                // CC
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    message.CC.Add(cc);
                }

                // BCC
                if (!string.IsNullOrWhiteSpace(bcc))
                {
                    message.Bcc.Add(bcc);
                }

                // Subject & Body
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                // Attachments
                if (attachments != null && attachments.Any())
                {
                    foreach (var attachmentPath in attachments)
                    {
                        if (File.Exists(attachmentPath))
                        {
                            message.Attachments.Add(new Attachment(attachmentPath));
                        }
                        else
                        {
                            _logger.LogWarning($"Attachment file not found: {attachmentPath}");
                        }
                    }
                }

                await client.SendMailAsync(message);
                _logger.LogInformation($"Email sent successfully to {to} with subject: {subject}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}. Error: {ex.Message}");
                return false;
            }
        }
    }
}
