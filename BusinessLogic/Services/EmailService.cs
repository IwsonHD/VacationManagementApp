using BusinessLogic.Interfaces;
using System.Net.Mail;
//using System.Reflection.Metadata;
//using NuGet.Configuration;
using System.Net;
using Microsoft.AspNetCore.Identity;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using BusinessLogic.DataBasesContext;


namespace BusinessLogic.Services
{
    public class EmailService: IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly UserManager<User> _userManager;
        private readonly VacationManagerDbContext _db;
        public EmailService(IConfiguration configuration,
               UserManager<User> userManager,
               VacationManagerDbContext db
            ) { 
            _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            _userManager = userManager;
            _db = db;
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

            var user = await _userManager.FindByEmailAsync(email);
            var mailToBeSaved = new EmailSent
            {
                Subject = title,
                Body = body,
                User = user,
                UserId = user.Id,
                SentDate = DateTime.Now,
                Email = email,

            };
            
            await _db.Emails.AddAsync(mailToBeSaved);
            await _db.SaveChangesAsync();   
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
