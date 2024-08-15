using BusinessLogic.DataBasesContext;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.DTOs;   
using BusinessLogic.AssistanceClasses; 

namespace BusinessLogic.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly VacationManagerDbContext _db;

        public UserRepository(VacationManagerDbContext db)
        {
            _db = db;
        }


        public async Task<ServiceResult<EmployeeDTO>> GetEmployeeByEmailAsync(string email)
        {
            ServiceResult<EmployeeDTO> serviceResult = new ServiceResult<EmployeeDTO>();
            var employee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
            
            if (employee == null) {
                serviceResult.AppendError(string.Empty, "Such employee does not exist");
                return serviceResult;
            }

            var employeeDTO = new EmployeeDTO
            {
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                Role = "Employee",
                EmployersEmail = employee.EmployersEmail
            };
            serviceResult.Data = employeeDTO;   
            return serviceResult;
        }

        public async Task<ServiceResult<EmployerDTO>> GetEmployerByEmailAsync(string email)
        {
            ServiceResult<EmployerDTO> serviceResult = new ServiceResult<EmployerDTO>();
            var employer = await _db.Employers.FirstOrDefaultAsync(employee => employee.Email == email);
            
            if(employer == null) {
                serviceResult.AppendError(string.Empty, "Such employer does not exist");
                return serviceResult;
            }


            var employerDTO = new EmployerDTO
            {
                Email = employer.Email,
                CompanyName = employer.CompanyName,
                PhoneNumber = employer.PhoneNumber,
                Role = "Employer",
                FirstName = employer.FirstName,
                LastName = employer.LastName
            };

            serviceResult.Data = employerDTO;

            return serviceResult;
        }   

        



    }
}
