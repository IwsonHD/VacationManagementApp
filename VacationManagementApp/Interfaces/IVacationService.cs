using VacationManagementApp.Models;
using VacationManagementApp.Dto;

namespace VacationManagementApp.Interfaces
{
    public interface IVacationService
    {
        IEnumerable<Vacation> GetVacations();

        Task<bool> AddVacationToDb(VacationDto vacation);
        IEnumerable<Vacation> GetYoursEmployeeVacation(string email);
        Vacation GetVacation(int? id);
        string EditVacation(Vacation editedVacation);
        
    }
}
