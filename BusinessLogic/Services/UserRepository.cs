using BusinessLogic.DataBasesContext;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly VacationManagerDbContext _db;

        public UserRepository(VacationManagerDbContext db)
        {
            _db = db;
        }


        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
            
            return employee;
        }

        public async Task<Employer?> GetEmployerByEmailAsync(string email)
        {
            var employer = await _db.Employers.FirstOrDefaultAsync(employee => employee.Email == email);   

            return employer;
        }   




    }
}
