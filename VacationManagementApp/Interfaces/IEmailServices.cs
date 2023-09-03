namespace VacationManagementApp.Interfaces
{
    public interface IEmailServices
    {
        Task SendEmail(string email, string title, string message);
        Task<bool> ConfirmEmail(string userId, string token);
    }
}
