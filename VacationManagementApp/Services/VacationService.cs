using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using VacationManagementApp.DataBases;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;
using VacationManagementApp.Dto;

namespace VacationManagementApp.Services
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


        public IEnumerable<Vacation> GetVacations()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userVacations = _db.Vacations
                .Where(v => v.EmployeeId == currentUser)
                .ToList();
            return userVacations;
        }

        public async Task<bool> AddVacationToDb(VacationDto vacationDto)
        {
            
            Vacation vacation = new Vacation
            {
                HowManyDays = vacationDto.HowManyDays,
                When = vacationDto.When,
                EmployeeId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            
            if (_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                
                await _db.Vacations.AddAsync(vacation);
                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }


        public IEnumerable<Vacation> GetYoursEmployeeVacation(string email)
        {
            Employee? employee = _db.Employees.FirstOrDefault(emp => emp.Email == email);

            if (employee == null)
            {
                return null;
            }
            
          
            IEnumerable<Vacation> employeesVacation = _db.Vacations
                .Where(v => v.EmployeeId == employee.Id)
                .ToList();
            return employeesVacation;
            
        }

        public Vacation GetVacation(int? id)
        {

            if(id == null || id == 0) return null;
            return _db.Vacations.Find(id);
        }

        public string? EditVacation(Vacation editedVacation)
        {
            _actionContextAccessor.ActionContext.ModelState.Remove("Employee");

            if (_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                string Email = _db.Employees.SingleOrDefault(e => e.Id == editedVacation.EmployeeId).Email;

                _db.Vacations.Update(editedVacation);
                _db.SaveChanges();
                return Email;
                
            }
            return null;

        }



    }
}
