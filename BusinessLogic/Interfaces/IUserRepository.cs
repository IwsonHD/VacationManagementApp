using BusinessLogic.Models;
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
        
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<Employer?> GetEmployerByEmailAsync(string email);
    }
}
