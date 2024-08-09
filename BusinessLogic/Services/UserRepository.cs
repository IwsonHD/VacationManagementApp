using BusinessLogic.DataBasesContext;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.DTOs;   
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


        public async Task<EmployeeDTO?> GetEmployeeByEmailAsync(string email)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
            var employeeDTO = new EmployeeDTO
            {
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                Role = "Employee",
                EmployersEmail = employee.EmployersEmail
            };
            return employeeDTO;
        }

        public async Task<EmployerDTO?> GetEmployerByEmailAsync(string email)
        {
            var employer = await _db.Employers.FirstOrDefaultAsync(employee => employee.Email == email);   
            var employerDTO = new EmployerDTO
            {
                Email = employer.Email,
                CompanyName = employer.CompanyName,
                PhoneNumber = employer.PhoneNumber,
                Role = "Employer",
                FirstName = employer.FirstName,
                LastName = employer.LastName
            };

            return employerDTO;
        }   

        



    }
}
