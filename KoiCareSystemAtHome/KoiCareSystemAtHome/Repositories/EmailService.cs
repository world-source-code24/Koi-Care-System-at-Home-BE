using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace KoiCareSystemAtHome.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Server)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSSL,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendVerificationEmailAsync(string email, string verificationLink)
        {
            string subject = "Xác thực tài khoản của bạn";
            string body = $@"
                <html>
                <body>
                    <h2>Xác thực tài khoản</h2>
                    <p>Cảm ơn bạn đã đăng ký tài khoản. Vui lòng nhấp vào liên kết dưới đây để xác thực email của bạn:</p>
                    <p><a href='{verificationLink}'>Xác thực tài khoản</a></p>
                    <p>Nếu bạn không thể nhấp vào liên kết, vui lòng sao chép và dán URL sau vào trình duyệt của bạn:</p>
                    <p>{verificationLink}</p>
                    <p>Nếu bạn không yêu cầu tạo tài khoản này, vui lòng bỏ qua email này.</p>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }
    }
}