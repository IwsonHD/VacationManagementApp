using VacationManagementApp.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using VacationManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.WebSockets;
using Org.BouncyCastle.Asn1.X509;

namespace VacationManagementApp.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        private class EmailSettings
        {
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }


        public EmailServices(IConfiguration configuration,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SendEmail(string email, string title, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("VacationManagementApp", emailSettings.Username));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = title;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;

            emailMessage.Body = bodyBuilder.ToMessageBody();
            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort,true);
            await smtpClient.AuthenticateAsync(emailSettings.Username, emailSettings.Password);
            await smtpClient.SendAsync(emailMessage);
            await smtpClient.DisconnectAsync(true);




        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                return true;
            }

            return false;
             
        }

  
    }
}
