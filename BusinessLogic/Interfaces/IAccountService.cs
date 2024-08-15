using Microsoft.AspNetCore.Mvc.ModelBinding;
using BusinessLogic.Models;
using BusinessLogic.DTOs;
using BusinessLogic.AssistanceClasses;

namespace BusinessLogic.Interfaces
{
       
    public interface IAccountService
    {
        /// <summary>
        /// Function witch takes care of a registration process
        /// </summary>
        /// <param name="model">Model of an account</param>
        /// <returns>true on success and false on failure</returns>
        public Task<ServiceResult<bool>> RegisterUserAsync(RegisterDto model);
        public Task<ServiceResult<String?>> LoginUserAsync(LoginDto model);

        public Task<ServiceResult<String>> LoginUserJWTAsync(LoginDto model);    

        public Task<ServiceResult<bool>> AcceptNewUserAsync(string newUsersEmail);


    }

    
}
