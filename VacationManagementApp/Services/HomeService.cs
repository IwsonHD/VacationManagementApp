using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;

namespace VacationManagementApp.Services
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
                    .ToList();
                return yourEmployees;

            }
            return null;
        }



    }
}
