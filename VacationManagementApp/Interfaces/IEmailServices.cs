namespace VacationManagementApp.Interfaces
{
    public interface IEmailServices
    {
        Task SendEmailConfirmationAsync(string email, string title, string message);
        Task<bool> ConfirmEmail(string userId, string token);
    }
}
