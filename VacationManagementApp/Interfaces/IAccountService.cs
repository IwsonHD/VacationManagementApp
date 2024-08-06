using Microsoft.AspNetCore.Mvc.ModelBinding;
using VacationManagementApp.Models;
using VacationManagementApp.Dto;
using VacationManagementApp.AssistanceClasses;

namespace VacationManagementApp.Interfaces
{
       
    public interface IAccountService
    {
        /// <summary>
        /// Function witch takes care of a registration process
        /// </summary>
        /// <param name="model">Model of an account</param>
        /// <returns>true on success and false on failure</returns>
        public Task<ServiceResult<bool>> RegisterUser(RegisterDto model);
        public Task<ServiceResult<String?>> LoginUser(LoginDto model);

        public Task<ServiceResult<bool>> AcceptNewUser(string newUsersEmail);
    }

    
}
