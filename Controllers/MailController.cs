using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs.MailDto;
using crm_api.Interfaces;
using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly ILogger<MailController> _logger;

        public MailController(IMailService mailService, ILogger<MailController> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        /// <summary>
        /// Send email immediately (synchronous)
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendMailDto dto)
        {
            try
            {
                var result = await _mailService.SendEmailAsync(
                    dto.To,
                    dto.Subject,
                    dto.Body,
                    dto.FromEmail,
                    dto.FromName,
                    dto.IsHtml,
                    dto.Cc,
                    dto.Bcc,
                    dto.Attachments
                );

                if (result)
                {
                    return Ok(new { message = "Email sent successfully", success = true });
                }

                return BadRequest(new { message = "Failed to send email", success = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return StatusCode(500, new { message = "An error occurred while sending email", error = ex.Message });
            }
        }

        /// <summary>
        /// Send email via Hangfire background job (asynchronous)
        /// </summary>
        [HttpPost("send-async")]
        public IActionResult SendEmailAsync([FromBody] SendMailDto dto)
        {
            try
            {
                var jobId = BackgroundJob.Enqueue<IMailJob>(job =>
                    job.SendEmailWithAttachmentsAsync(
                        dto.To,
                        dto.Subject,
                        dto.Body,
                        dto.FromEmail,
                        dto.FromName,
                        dto.IsHtml,
                        dto.Cc,
                        dto.Bcc,
                        dto.Attachments
                    )
                );

                return Ok(new
                {
                    message = "Email queued for sending",
                    success = true,
                    jobId = jobId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error queuing email");
                return StatusCode(500, new { message = "An error occurred while queuing email", error = ex.Message });
            }
        }

        /// <summary>
        /// Send email to multiple recipients via Hangfire background job
        /// </summary>
        [HttpPost("send-bulk")]
        public IActionResult SendBulkEmail([FromBody] BulkSendMailDto dto)
        {
            try
            {
                var jobIds = new List<string>();

                foreach (var recipient in dto.To)
                {
                    var jobId = BackgroundJob.Enqueue<IMailJob>(job =>
                        job.SendEmailWithAttachmentsAsync(
                            recipient,
                            dto.Subject,
                            dto.Body,
                            dto.FromEmail,
                            dto.FromName,
                            dto.IsHtml,
                            dto.Cc,
                            dto.Bcc,
                            dto.Attachments
                        )
                    );
                    jobIds.Add(jobId);
                }

                return Ok(new
                {
                    message = $"{dto.To.Count} emails queued for sending",
                    success = true,
                    jobIds = jobIds,
                    count = jobIds.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error queuing bulk emails");
                return StatusCode(500, new { message = "An error occurred while queuing emails", error = ex.Message });
            }
        }

        /// <summary>
        /// Schedule email to be sent at a specific time
        /// </summary>
        [HttpPost("schedule")]
        public IActionResult ScheduleEmail([FromBody] SendMailDto dto, [FromQuery] DateTime scheduleAt)
        {
            try
            {
                if (scheduleAt <= DateTime.UtcNow)
                {
                    return BadRequest(new { message = "Schedule time must be in the future", success = false });
                }

                var delay = scheduleAt - DateTime.UtcNow;
                var jobId = BackgroundJob.Schedule<IMailJob>(job =>
                    job.SendEmailWithAttachmentsAsync(
                        dto.To,
                        dto.Subject,
                        dto.Body,
                        dto.FromEmail,
                        dto.FromName,
                        dto.IsHtml,
                        dto.Cc,
                        dto.Bcc,
                        dto.Attachments
                    ),
                    delay
                );

                return Ok(new
                {
                    message = "Email scheduled for sending",
                    success = true,
                    jobId = jobId,
                    scheduledAt = scheduleAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling email");
                return StatusCode(500, new { message = "An error occurred while scheduling email", error = ex.Message });
            }
        }
    }
}
