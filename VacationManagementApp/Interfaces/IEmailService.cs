namespace VacationManagementApp.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(string email, string title, string body);
        Task<bool> ConfirmaEmailAsync(string userId, string Token);
    }
}
