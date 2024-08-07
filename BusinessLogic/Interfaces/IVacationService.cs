using BusinessLogic.Models;
using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
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
