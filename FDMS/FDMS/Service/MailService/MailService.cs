using FDMS.Entity;
using FDMS.Model;
using FDMS.Service.JWTService;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FDMS.Service.MailService
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly FDMSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MailService(IOptions<MailSettings> mailSettingsOptions, FDMSContext context
            , IHttpContextAccessor httpContextAccessor, IJWTService jWTService)
        {
            this._mailSettings = mailSettingsOptions.Value;
            this._context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<APIResponse> SendMail(MailData mailData)
        {          
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email.Equals(mailData.ReceiverEmail));
            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(mailData.ReceiverName, mailData.ReceiverEmail);
                emailMessage.To.Add(emailTo);
                //thêm mail CC và BCC nếu cần
                //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                emailMessage.Subject = "FDMS - New password"; /*mailData.Title*/;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody =
                    "Renew password:"
                    + "<br/>Xin chào: " + user.Name.ToString()
                    + "<br/>Đây là mật khẩu mới: " + mailData.Body + "</b>";

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.SmtpServer, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }
            return new APIResponse { success = true, message = "Send feedback success." };
        }
    }
}
