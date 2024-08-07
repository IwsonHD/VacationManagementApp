using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BusinessLogic.AssistanceClasses;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public class HomeService: IHomeService
    {
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        public HomeService(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor
            ) { _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
            }

        public async Task<User>? ShowYourData()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                
                return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            }

            return null;

        }

        public async Task<IEnumerable<Employee>>? ShowYourEmployees()

        {
            if (_httpContextAccessor.HttpContext.User.IsInRole("Employer"))
            {
                var you = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                IEnumerable<Employee> yourEmployees = _userManager.Users
                    .OfType<Employee>()
                    .Where(e => e.EmployersEmail == you.Email)
                    .Where(e => e.EmployeeConfirmed)
                    .ToList();
                return yourEmployees;

            }
            return null;
        }

        public async Task<ServiceResult<IEnumerable<Employee>>>? ShowNewEmployees()
        {
            ServiceResult<IEnumerable<Employee>> serviceResult = new ServiceResult<IEnumerable<Employee>>(); 
            var you = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(you == null)
            {
                serviceResult.AppendError(string.Empty, "Your employer does not exist contact the help center");
                return serviceResult;
            }
            serviceResult.Data = _userManager.Users
                .OfType<Employee>()
                .Where(e => e.EmployersEmail == you.Email)
                .Where(e => !e.EmployeeConfirmed)
                .ToList();
            return serviceResult;

        }    
            


    }
}
