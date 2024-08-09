using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interfaces;

namespace VacationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Trasa do akcji będzie 'api/Users/employee/{employeeEmail}'
        [HttpGet("employee/{employeeEmail}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployee(string employeeEmail)
        {
            var employee = await _userRepository.GetEmployeeByEmailAsync(employeeEmail);

            if (employee == null) return NotFound();

            return Ok(employee);
        }

        // Trasa do akcji będzie 'api/employer/{employeeEmail}'
        [HttpGet("employer/{employerEmail}")]
        [ProducesResponseType(200, Type = typeof(Employer))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployer(string employerEmail)
        {
            var employer = await _userRepository.GetEmployerByEmailAsync(employerEmail);

            if(employer == null) return NotFound();

            return Ok(employer);
        }
    }
}
