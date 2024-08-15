using BusinessLogic.Models;
using BusinessLogic.DTOs;
using BusinessLogic.AssistanceClasses;


namespace BusinessLogic.Interfaces
{
    public interface IVacationService
    {
        
        ServiceResult<IEnumerable<Vacation>> GetVacations();

        Task<ServiceResult<bool>> AddVacationToDb(VacationDto vacation);
        ServiceResult<IEnumerable<Vacation>> GetYoursEmployeeVacation(string email);
        ServiceResult<Vacation> GetVacation(int? id);
        ServiceResult<string> EditVacation(Vacation editedVacation);
        
    }
}
