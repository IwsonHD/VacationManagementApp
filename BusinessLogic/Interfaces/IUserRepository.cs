using BusinessLogic.Models;
using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.Interfaces
{
    public interface IUserRepository
    {

        //Do zamiany na service result prawdopodobnie
        
        Task<EmployeeDTO?> GetEmployeeByEmailAsync(string email);
        Task<EmployerDTO?> GetEmployerByEmailAsync(string email);
    }
}
