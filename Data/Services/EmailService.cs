using Data.Entities.Service;
using Data.Extensions;
using Data.Interfaces;
using Data.Services.utility;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepoService repo;
        private readonly SmtpSettingsModel smtpSetting;
        public EmailService(IRepoService _repo)
        {
            repo = _repo;
            smtpSetting = GeneralSettings.SmtpSettings ?? GeneralSettings.GetSmtpSettings(repo);
        }

        public async Task<object[]> SendEmailAsync(string emails, string subject, string htmlMessage, string emailsCC = null)
        {
            try
            {
                var emailToList = GetEmailList(emails);
                if (emailToList.Count == 0)
                    return new object[] { false, "Email to is null" };
                var emailCCList = GetEmailList(emailsCC);

                var sendEmail = new MimeMessage
                {
                    Subject = subject,
                    Body = new TextPart(TextFormat.Html) { Text = htmlMessage }
                };

                sendEmail.From.Add(new MailboxAddress(smtpSetting.SmtpFromDisplayName, smtpSetting.SmtpFromAddress));
                sendEmail.To.AddRange(emailToList);
                if (emailCCList.Count > 0)
                    sendEmail.Cc.AddRange(emailCCList);

                using var smtp = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) => true
                };

                await smtp.ConnectAsync(smtpSetting.SmtpServer, smtpSetting.SmtpPort.Value, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(smtpSetting.SmtpUserName ?? smtpSetting.SmtpFromAddress, smtpSetting.SmtpPassword);
                smtp.Send(sendEmail);
                await smtp.DisconnectAsync(true);
                return new object[] { true, "Email Sent" };
            }
            catch (Exception ex)
            {
                return new object[] { false, ex.Message };
            }
        }


        public InternetAddressList GetEmailList(string emails)
        {
            var emailList = new InternetAddressList();
            if (!string.IsNullOrWhiteSpace(emails))
            {
                var emailArray = emails.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var email in emailArray)
                {
                    emailList.Add(new MailboxAddress(email, email));
                }

            }
            return emailList;
        }
    }
}
