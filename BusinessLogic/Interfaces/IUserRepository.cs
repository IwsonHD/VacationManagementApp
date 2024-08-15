using BusinessLogic.DTOs;
using BusinessLogic.AssistanceClasses;

namespace BusinessLogic.Interfaces
{
    public interface IUserRepository
    {

        
        Task<ServiceResult<EmployeeDTO>> GetEmployeeByEmailAsync(string email);
        Task<ServiceResult<EmployerDTO>> GetEmployerByEmailAsync(string email);
    }
}
