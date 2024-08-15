using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using BusinessLogic.DataBasesContext;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Http;
using BusinessLogic.AssistanceClasses;

namespace BusinessLogic.Services
{
    public class VacationService: IVacationService
    {
        private readonly VacationManagerDbContext _db;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _actionContextAccessor;

        public VacationService(VacationManagerDbContext db,
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
        }


        public ServiceResult<IEnumerable<Vacation>> GetVacations()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serviceResult = new ServiceResult<IEnumerable<Vacation>>();

            if (currentUser == null)
            {
                serviceResult.AppendError(String.Empty, "No such user exists");
                return serviceResult;
            }
            
            var userVacations = _db.Vacations
                .Where(v => v.EmployeeId == currentUser)
                .ToList();
            serviceResult.Data = userVacations;


            return serviceResult;
        }

        public async Task<ServiceResult<bool>> AddVacationToDb(VacationDto vacationDto)
        {
            var serviceResult = new ServiceResult<bool>();  
            Vacation vacation = new Vacation
            {
                HowManyDays = vacationDto.HowManyDays,
                When = vacationDto.When,
                EmployeeId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            await _db.Vacations.AddAsync(vacation);
            await _db.SaveChangesAsync();

            return serviceResult;

        }


        public ServiceResult<IEnumerable<Vacation>> GetYoursEmployeeVacation(string email)
        {
            var serviceResult = new ServiceResult<IEnumerable<Vacation>>();

            Employee? employee = _db.Employees.FirstOrDefault(emp => emp.Email == email);

            if (employee == null)
            {
                serviceResult.AppendError(String.Empty, "No such employee exists");
                return serviceResult;
            }
            
          
            IEnumerable<Vacation> employeesVacation = _db.Vacations
                .Where(v => v.EmployeeId == employee.Id)
                .ToList();

            serviceResult.Data = employeesVacation;

            return serviceResult;
            
        }

        public ServiceResult<Vacation> GetVacation(int? id)
        {
            var serviceResult = new ServiceResult<Vacation>();

            if (id == null || id == 0) {
                serviceResult.AppendError(String.Empty, "id is null");
            }
            serviceResult.Data = _db.Vacations.Find(id);
            return serviceResult;
        }

        public ServiceResult<string> EditVacation(Vacation editedVacation)
        {
            var serviceResult = new ServiceResult<string>();

            string Email = _db.Employees.SingleOrDefault(e => e.Id == editedVacation.EmployeeId).Email;

            if (Email == null)
            {
                serviceResult.AppendError(String.Empty, "Unknown user error");
                return serviceResult;
            }
            serviceResult.Data = Email;
            _db.Vacations.Update(editedVacation);
            _db.SaveChanges();
            

            return serviceResult;

        }



    }
}
