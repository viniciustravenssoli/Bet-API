using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Bet.Application.Services.Email;

namespace EAC.Application.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailConfiguration;

        public EmailService(IOptions<EmailSettings> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task ConfirmationBetWinner(string body, string earnedValue, string emailto)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_emailConfiguration.Username));
                email.To.Add(MailboxAddress.Parse(emailto));
                email.Subject = $"Parabens voce ganhou uma aposta e seu ganho foi de {earnedValue}";
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
