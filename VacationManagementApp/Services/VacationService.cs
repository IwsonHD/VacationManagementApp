using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using VacationManagementApp.DataBases;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;

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


        public IEnumerable<Vacation> GetVacations(string? currentUserId)
        { 
            var userVacations = _db.Vacations
                .Where(v => v.EmployeeId == currentUserId)
                .ToList();

            return userVacations;
        }

        public async void AddVacationToDb(Vacation vacation)
        {

           await _db.Vacations.AddAsync(vacation);
           _db.SaveChanges();             
      
        }


        public IEnumerable<Vacation> GetYoursEmployeeVacation(string email)
        {
            Employee? employee = _db.Employees.FirstOrDefault(emp => emp.Email == email);

            if (employee == null)
            {
                return null;
            }

            var employeesVacation = _db.Vacations
                .Where(v => v.EmployeeId == employee.Id)
                .ToList();

            return employeesVacation;
        }

        public Vacation GetVacation(int? id)
        {

            if(id == null || id == 0) return null;
            return _db.Vacations.Find(id);
        }
        //zwaliduj w kontrolerze za pomoca validatora a tutaj tylko dodam do bazy danch
        public string? EditVacation(Vacation editedVacation)
        {
      
            string Email = _db.Employees.SingleOrDefault(e => e.Id == editedVacation.EmployeeId).Email;

            _db.Vacations.Update(editedVacation);
            _db.SaveChanges();
            return Email;
            
    

        }



    }
}
