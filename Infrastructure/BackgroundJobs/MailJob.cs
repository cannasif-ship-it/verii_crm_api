using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using crm_api.Interfaces;
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

        public async Task SendUserCreatedEmailAsync(string email, string username, string password, string? firstName, string? lastName, string baseUrl)
        {
            var emailSubject = "Kullanıcınız oluşturulmuştur";
            var displayName = string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) 
                ? username 
                : $"{firstName} {lastName}".Trim();

            var content = $@"
                <p>Sayın {displayName},</p>
                <p>Kullanıcınız başarıyla oluşturulmuştur. Giriş bilgileriniz aşağıdadır:</p>
                <div class=""info-box"">
                    <p><strong>Login için E-postanız:</strong> {email}</p>
                    <p><strong>Şifreniz:</strong> {password}</p>
                </div>
                <p>Yukarıdaki bilgilerle giriş yapıp menü üzerinden kullanıcı şifrenizi değiştirebilirsiniz.</p>
                <div style=""text-align: center; margin-top: 30px;"">
                    <a href=""{baseUrl}"" class=""btn"">Giriş Yap</a>
                </div>";

            var emailBody = GetEmailTemplate("Kullanıcınız Oluşturuldu", content);
            await SendEmailAsync(email, emailSubject, emailBody, true);
        }

        public async Task SendPasswordResetEmailAsync(string email, string fullName, string resetLink, string emailSubject)
        {
            var content = $@"
                <p>Sayın {fullName},</p>
                <p>Şifre sıfırlama talebiniz alınmıştır. Aşağıdaki butona tıklayarak şifrenizi sıfırlayabilirsiniz:</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{resetLink}"" class=""btn"">Şifremi Sıfırla</a>
                </div>
                <p>Veya aşağıdaki linki tarayıcınıza kopyalayabilirsiniz:</p>
                <p style=""word-break: break-all; color: #fb923c; font-size: 14px;"">{resetLink}</p>
                <div style=""margin-top: 20px; padding-top: 20px; border-top: 1px solid rgba(255,255,255,0.1);"">
                    <p style=""font-size: 13px; color: #94a3b8; margin: 0;"">Bu link 30 dakika süreyle geçerlidir.</p>
                    <p style=""font-size: 13px; color: #94a3b8; margin: 5px 0 0 0;"">Eğer şifre sıfırlama talebinde bulunmadıysanız, lütfen bu e-postayı dikkate almayınız.</p>
                </div>";

            var emailBody = GetEmailTemplate("Şifre Sıfırlama Talebi", content);
            await SendEmailAsync(email, emailSubject, emailBody, true);
        }

        public async Task SendPasswordChangedEmailAsync(string email, string displayName, string baseUrl)
        {
            var emailSubject = "Şifreniz Güncellendi";
            var content = $@"
                <p>Sayın {displayName},</p>
                <p>Eski şifreniz başarılı şekilde güncellenmiştir.</p>
                <p>Hesabınıza güvenli şekilde devam edebilirsiniz.</p>
                <div style=""text-align: center; margin-top: 30px;"">
                    <a href=""{baseUrl}"" class=""btn"">Giriş Yap</a>
                </div>";

            var emailBody = GetEmailTemplate("Şifre Güncelleme Bildirimi", content);
            await SendEmailAsync(email, emailSubject, emailBody, true);
        }

        public async Task SendPasswordResetCompletedEmailAsync(string email, string displayName, string baseUrl)
        {
            var emailSubject = "Şifre Sıfırlama İşlemi Tamamlandı";
            var content = $@"
                <p>Sayın {displayName},</p>
                <p>Şifre resetleme işlemi başarılı şekilde tamamlanmıştır.</p>
                <p>Yeni şifreniz ile güvenli şekilde giriş yapabilirsiniz.</p>
                <div style=""text-align: center; margin-top: 30px;"">
                    <a href=""{baseUrl}"" class=""btn"">Giriş Yap</a>
                </div>";

            var emailBody = GetEmailTemplate("Şifre Sıfırlama Tamamlandı", content);
            await SendEmailAsync(email, emailSubject, emailBody, true);
        }

        public async Task SendDemandApprovalPendingEmailAsync(string email, string displayName, string subject, string approvalLink, string demandLink)
        {
            var content = $@"
                <p>Sayın {displayName},</p>
                <p>Onay bekleyen kaydınız bulunmaktadır. Aşağıdaki butonlarla işlemi onaylayabilir/reddedebilir veya detayları görüntüleyebilirsiniz.</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{approvalLink}"" class=""btn"">Onayla / Reddet</a>
                    <a href=""{demandLink}"" class=""btn btn-secondary"" style=""margin-left: 10px;"">Talebe Git</a>
                </div>";

            var body = GetEmailTemplate("Onay Bekleyen Kayıt", content);
            await SendEmailAsync(email, subject, body, true);
        }

        public async Task SendDemandApprovalPendingEmailsAsync(
            List<(string Email, string FullName, long UserId)> usersToNotify,
            Dictionary<long, long> userIdToActionId,
            string baseUrl,
            string approvalPath,
            string demandPath,
            long demandId)
        {
            var subject = "Onay Bekleyen Kaydınız Bulunmaktadır";
            var demandLink = $"{baseUrl}/{demandPath}/{demandId}";

            foreach (var (email, fullName, uid) in usersToNotify)
            {
                var displayName = string.IsNullOrWhiteSpace(fullName) ? "Değerli Kullanıcı" : fullName;
                var actionId = userIdToActionId.GetValueOrDefault(uid);
                var approvalLink = actionId != 0
                    ? $"{baseUrl}/{approvalPath}?actionId={actionId}"
                    : $"{baseUrl}/{approvalPath}";

                await SendDemandApprovalPendingEmailAsync(
                    email,
                    displayName,
                    subject,
                    approvalLink,
                    demandLink);
            }
        }

        public async Task SendOrderApprovalPendingEmailAsync(string email, string displayName, string subject, string approvalLink, string orderLink)
        {
            var content = $@"
                <p>Sayın {displayName},</p>
                <p>Onay bekleyen sipariş kaydı bulunmaktadır. Aşağıdaki butonlarla işlemi onaylayabilir/reddedebilir veya detayları görüntüleyebilirsiniz.</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{approvalLink}"" class=""btn"">Onayla / Reddet</a>
                    <a href=""{orderLink}"" class=""btn btn-secondary"" style=""margin-left: 10px;"">Siparişe Git</a>
                </div>";

            var body = GetEmailTemplate("Onay Bekleyen Sipariş", content);
            await SendEmailAsync(email, subject, body, true);
        }

        public async Task SendBulkOrderApprovalPendingEmailsAsync(
            List<(string Email, string FullName, long UserId)> usersToNotify,
            Dictionary<long, long> userIdToActionId,
            string baseUrl,
            string approvalPath,
            string orderPath,
            long orderId)
        {
            var subject = "Onay Bekleyen Kaydınız Bulunmaktadır";
            var orderLink = $"{baseUrl}/{orderPath}/{orderId}";

            foreach (var (email, fullName, uid) in usersToNotify)
            {
                var displayName = string.IsNullOrWhiteSpace(fullName) ? "Değerli Kullanıcı" : fullName;
                var actionId = userIdToActionId.GetValueOrDefault(uid);
                var approvalLink = actionId != 0
                    ? $"{baseUrl}/{approvalPath}?actionId={actionId}"
                    : $"{baseUrl}/{approvalPath}";

                await SendOrderApprovalPendingEmailAsync(
                    email,
                    displayName,
                    subject,
                    approvalLink,
                    orderLink);
            }
        }

        public async Task SendQuotationApprovalPendingEmailAsync(string email, string displayName, string subject, string approvalLink, string quotationLink)
        {
            var content = $@"
                <p>Sayın {displayName},</p>
                <p>Onay bekleyen teklif kaydı bulunmaktadır. Aşağıdaki butonlarla işlemi onaylayabilir/reddedebilir veya detayları görüntüleyebilirsiniz.</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{approvalLink}"" class=""btn"">Onayla / Reddet</a>
                    <a href=""{quotationLink}"" class=""btn btn-secondary"" style=""margin-left: 10px;"">Teklife Git</a>
                </div>";

            var body = GetEmailTemplate("Onay Bekleyen Teklif", content);
            await SendEmailAsync(email, subject, body, true);
        }

        public async Task SendBulkQuotationApprovalPendingEmailsAsync(
            List<(string Email, string FullName, long UserId)> usersToNotify,
            Dictionary<long, long> userIdToActionId,
            string baseUrl,
            string approvalPath,
            string quotationPath,
            long quotationId)
        {
            var subject = "Onay Bekleyen Kaydınız Bulunmaktadır";
            var quotationLink = $"{baseUrl}/{quotationPath}/{quotationId}";

            foreach (var (email, fullName, uid) in usersToNotify)
            {
                var displayName = string.IsNullOrWhiteSpace(fullName) ? "Değerli Kullanıcı" : fullName;
                var actionId = userIdToActionId.GetValueOrDefault(uid);
                var approvalLink = actionId != 0
                    ? $"{baseUrl}/{approvalPath}?actionId={actionId}"
                    : $"{baseUrl}/{approvalPath}";

                await SendQuotationApprovalPendingEmailAsync(
                    email,
                    displayName,
                    subject,
                    approvalLink,
                    quotationLink);
            }
        }

        public async Task SendQuotationApprovedEmailAsync(
            string creatorEmail,
            string creatorFullName,
            string approverFullName,
            string quotationNo,
            string quotationLink)
        {
            var subject = $"{quotationNo} Numaralı Teklifiniz Onaylanmıştır";
            var displayName = string.IsNullOrWhiteSpace(creatorFullName) ? "Değerli Kullanıcı" : creatorFullName;
            
            var content = $@"
                <p>Sayın {displayName},</p>
                <p><strong>{quotationNo}</strong> numaralı teklifiniz <strong>{approverFullName}</strong> tarafından onaylanmıştır.</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{quotationLink}"" class=""btn"">Teklifi Görüntüle</a>
                </div>";

            var body = GetEmailTemplate("Teklif Onaylandı", content);
            await SendEmailAsync(creatorEmail, subject, body, true);
        }

        public async Task SendQuotationRejectedEmailAsync(
            string creatorEmail,
            string creatorFullName,
            string rejectorFullName,
            string quotationNo,
            string rejectReason,
            string quotationLink)
        {
            var subject = $"{quotationNo} Numaralı Teklifiniz Reddedilmiştir";
            var displayName = string.IsNullOrWhiteSpace(creatorFullName) ? "Değerli Kullanıcı" : creatorFullName;
            
            var content = $@"
                <p>Sayın {displayName},</p>
                <p><strong>{quotationNo}</strong> numaralı teklifiniz <strong>{rejectorFullName}</strong> tarafından aşağıdaki gerekçe ile reddedilmiştir:</p>
                <div class=""info-box"">
                    <p><strong>Red Nedeni:</strong> {rejectReason}</p>
                </div>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{quotationLink}"" class=""btn"">Teklifi Görüntüle</a>
                </div>";

            var body = GetEmailTemplate("Teklif Reddedildi", content);
            await SendEmailAsync(creatorEmail, subject, body, true);
        }

        public async Task SendDemandApprovedEmailAsync(
            string creatorEmail,
            string creatorFullName,
            string approverFullName,
            string demandNo,
            string demandLink)
        {
            var subject = $"{demandNo} Numaralı Talebiniz Onaylanmıştır";
            var displayName = string.IsNullOrWhiteSpace(creatorFullName) ? "Değerli Kullanıcı" : creatorFullName;
            
            var content = $@"
                <p>Sayın {displayName},</p>
                <p><strong>{demandNo}</strong> numaralı talebiniz <strong>{approverFullName}</strong> tarafından onaylanmıştır.</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{demandLink}"" class=""btn"">Talebi Görüntüle</a>
                </div>";

            var body = GetEmailTemplate("Talep Onaylandı", content);
            await SendEmailAsync(creatorEmail, subject, body, true);
        }

        public async Task SendDemandRejectedEmailAsync(
            string creatorEmail,
            string creatorFullName,
            string rejectorFullName,
            string demandNo,
            string rejectReason,
            string demandLink)
        {
            var subject = $"{demandNo} Numaralı Talep Reddedilmiştir";
            var displayName = string.IsNullOrWhiteSpace(creatorFullName) ? "Değerli Kullanıcı" : creatorFullName;
            
            var content = $@"
                <p>Sayın {displayName},</p>
                <p><strong>{demandNo}</strong> numaralı talebiniz <strong>{rejectorFullName}</strong> tarafından aşağıdaki gerekçe ile reddedilmiştir:</p>
                <div class=""info-box"">
                    <p><strong>Red Nedeni:</strong> {rejectReason}</p>
                </div>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{demandLink}"" class=""btn"">Talebi Görüntüle</a>
                </div>";

            var body = GetEmailTemplate("Talep Reddedildi", content);
            await SendEmailAsync(creatorEmail, subject, body, true);
        }

        public async Task SendOrderApprovedEmailAsync(
            string creatorEmail,
            string creatorFullName,
            string approverFullName,
            string orderNo,
            string orderLink)
        {
            var subject = $"{orderNo} Numaralı Siparişiniz Onaylanmıştır";
            var displayName = string.IsNullOrWhiteSpace(creatorFullName) ? "Değerli Kullanıcı" : creatorFullName;
            
            var content = $@"
                <p>Sayın {displayName},</p>
                <p><strong>{orderNo}</strong> numaralı siparişiniz <strong>{approverFullName}</strong> tarafından onaylanmıştır.</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{orderLink}"" class=""btn"">Siparişi Görüntüle</a>
                </div>";

            var body = GetEmailTemplate("Sipariş Onaylandı", content);
            await SendEmailAsync(creatorEmail, subject, body, true);
        }

        public async Task SendOrderRejectedEmailAsync(
            string creatorEmail,
            string creatorFullName,
            string rejectorFullName,
            string orderNo,
            string rejectReason,
            string orderLink)
        {
            var subject = $"{orderNo} Numaralı Sipariş Reddedilmiştir";
            var displayName = string.IsNullOrWhiteSpace(creatorFullName) ? "Değerli Kullanıcı" : creatorFullName;
            
            var content = $@"
                <p>Sayın {displayName},</p>
                <p><strong>{orderNo}</strong> numaralı siparişiniz <strong>{rejectorFullName}</strong> tarafından aşağıdaki gerekçe ile reddedilmiştir:</p>
                <div class=""info-box"">
                    <p><strong>Red Nedeni:</strong> {rejectReason}</p>
                </div>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{orderLink}"" class=""btn"">Siparişi Görüntüle</a>
                </div>";

            var body = GetEmailTemplate("Sipariş Reddedildi", content);
            await SendEmailAsync(creatorEmail, subject, body, true);
        }

        private string GetEmailTemplate(string title, string content)
        {
            var year = DateTime.Now.Year;
            return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<link href=""https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600;700&display=swap"" rel=""stylesheet"">
<style>
    body {{ font-family: 'Outfit', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #0f0518; margin: 0; padding: 0; color: #ffffff; }}
    .wrapper {{ width: 100%; table-layout: fixed; background-color: #0f0518; padding-bottom: 40px; }}
    .container {{ max-width: 600px; margin: 0 auto; background-color: #140a1e; border-radius: 24px; border: 1px solid rgba(255,255,255,0.1); overflow: hidden; box-shadow: 0 20px 40px rgba(0,0,0,0.4); }}
    .header {{ padding: 40px 40px 20px 40px; text-align: center; background: radial-gradient(circle at 50% -20%, rgba(236, 72, 153, 0.15), transparent 70%); }}
    .header h2 {{ margin: 0; font-size: 24px; font-weight: 700; color: #ffffff; text-transform: uppercase; letter-spacing: 1px; }}
    .content {{ padding: 20px 40px 40px 40px; color: #e2e8f0; line-height: 1.6; font-size: 16px; }}
    .footer {{ padding: 20px; text-align: center; color: #64748b; font-size: 12px; border-top: 1px solid rgba(255,255,255,0.05); background-color: #0c0516; }}
    .btn {{ display: inline-block; padding: 14px 32px; color: #ffffff !important; text-decoration: none; border-radius: 12px; font-weight: bold; text-transform: uppercase; letter-spacing: 1px; margin: 10px 5px; background: #f97316; background: linear-gradient(90deg, #db2777, #f97316, #eab308); box-shadow: 0 4px 15px rgba(249, 115, 22, 0.3); transition: all 0.3s ease; }}
    .btn:hover {{ opacity: 0.9; transform: translateY(-2px); box-shadow: 0 6px 20px rgba(249, 115, 22, 0.4); }}
    .btn-secondary {{ background: transparent; border: 1px solid rgba(255,255,255,0.2); color: #e2e8f0 !important; box-shadow: none; }}
    .btn-secondary:hover {{ background: rgba(255,255,255,0.05); border-color: rgba(255,255,255,0.4); }}
    .info-box {{ background-color: rgba(0,0,0,0.3); padding: 20px; border-radius: 12px; margin: 20px 0; border: 1px solid rgba(255,255,255,0.1); }}
    strong {{ color: #fb923c; }}
    a {{ color: #fb923c; text-decoration: none; }}
    p {{ margin-bottom: 15px; }}
</style>
</head>
<body>
    <div class=""wrapper"">
        <br>
        <div class=""container"">
            <div class=""header"">
                <h2>{title}</h2>
            </div>
            <div class=""content"">
                {content}
            </div>
            <div class=""footer"">
                <p>Bu e-posta otomatik olarak gönderilmiştir, lütfen yanıtlamayınız.</p>
                <p>&copy; {year} v3rii CRM</p>
            </div>
        </div>
        <br>
    </div>
</body>
</html>";
        }
    }
}
