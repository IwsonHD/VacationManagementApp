using BusinessLogic.Interfaces;
using System.Net.Mail;
//using System.Reflection.Metadata;
//using NuGet.Configuration;
using System.Net;
using Microsoft.AspNetCore.Identity;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;


namespace BusinessLogic.Services
{
    public class EmailService: IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly UserManager<User> _userManager;
        public EmailService(IConfiguration configuration,
               UserManager<User> userManager) { 
            _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            _userManager = userManager;
        }

        public async Task SendEmailConfirmationAsync(string email, string title, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = title,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(email));

            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true

            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
        public async Task<bool> ConfirmaEmailAsync(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded;
        }







        private class SmtpSettings
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
