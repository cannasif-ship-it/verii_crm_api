using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using cms_webapi.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 30, 60, 120 })]
    public class MailJob : IMailJob
    {
        private readonly IMailService _mailService;
        private readonly ILogger<MailJob> _logger;

        public MailJob(IMailService mailService, ILogger<MailJob> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, string? cc = null, string? bcc = null, List<string>? attachments = null)
        {
            try
            {
                _logger.LogInformation($"MailJob: Sending email to {to} with subject: {subject}");
                
                var result = await _mailService.SendEmailAsync(to, subject, body, isHtml, cc, bcc, attachments);
                
                if (result)
                {
                    _logger.LogInformation($"MailJob: Email sent successfully to {to}");
                }
                else
                {
                    _logger.LogWarning($"MailJob: Failed to send email to {to}");
                    throw new Exception($"Failed to send email to {to}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"MailJob: Error sending email to {to}");
                throw;
            }
        }

        public async Task SendEmailWithAttachmentsAsync(string to, string subject, string body, string? fromEmail = null, string? fromName = null, bool isHtml = true, string? cc = null, string? bcc = null, List<string>? attachments = null)
        {
            try
            {
                _logger.LogInformation($"MailJob: Sending email to {to} with subject: {subject}");
                
                var result = await _mailService.SendEmailAsync(to, subject, body, fromEmail, fromName, isHtml, cc, bcc, attachments);
                
                if (result)
                {
                    _logger.LogInformation($"MailJob: Email sent successfully to {to}");
                }
                else
                {
                    _logger.LogWarning($"MailJob: Failed to send email to {to}");
                    throw new Exception($"Failed to send email to {to}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"MailJob: Error sending email to {to}");
                throw;
            }
        }
    }
}
