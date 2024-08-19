using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interfaces;
using BusinessLogic.AssistanceClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        [ProducesResponseType(401)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployee(string employeeEmail)
        {
            var serviceResult= await _userRepository.GetEmployeeByEmailAsync(employeeEmail);

            if (!serviceResult.Succeed) return NotFound();

            return Ok(serviceResult.Data);
        }

        [HttpGet("employer/{employerEmail}")]
        [ProducesResponseType(200, Type = typeof(Employer))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployer(string employerEmail)
        {
            var serviceResult = await _userRepository.GetEmployerByEmailAsync(employerEmail);

            if (!serviceResult.Succeed) return NotFound();

            return Ok(serviceResult.Data);
        }
    }
}
