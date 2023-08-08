using Microsoft.AspNetCore.Mvc.ModelBinding;
using VacationManagementApp.Models;
using VacationManagementApp.Dto;

namespace VacationManagementApp.Interfaces
{
       
    public interface IAccountService
    {
        /// <summary>
        /// Function witch takes care of a registration process
        /// </summary>
        /// <param name="model">Model of an account</param>
        /// <returns>true on success and false on failure</returns>
        public Task<bool> RegisterUser(RegisterDto model);
        public Task<bool> LoginUser(LoginDto model);
        
    }

    
}
