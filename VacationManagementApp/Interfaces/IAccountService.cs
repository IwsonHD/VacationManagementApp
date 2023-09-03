using Microsoft.AspNetCore.Mvc.ModelBinding;
using VacationManagementApp.Models;
using VacationManagementApp.Dto;
using Microsoft.AspNetCore.Mvc;
using VacationManagementApp.Validators;

namespace VacationManagementApp.Interfaces
{
       
    public interface IAccountService
    {
        /// <summary>
        /// Function witch takes care of a registration process
        /// </summary>
        /// <param name="model">Model of an account</param>
        /// <returns>true on success and false on failure</returns>
        public Task<ServiceResult> RegisterUser(RegisterDto model);
        public Task<ServiceResult> LoginUser(LoginDto model);
        public Task<bool> ConfirmEmployee(string userId, string token);
        
    }

    
}
