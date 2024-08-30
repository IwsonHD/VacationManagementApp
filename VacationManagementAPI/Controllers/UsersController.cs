using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interfaces;
using BusinessLogic.AssistanceClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MediatR;
using BusinessLogic.CQRS.Queries.GetEmployer;
using BusinessLogic.CQRS.Queries.GetEmployee;
using BusinessLogic.DTOs;
using BusinessLogic.CQRS.Queries.GetYourEmployees;

namespace VacationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ISender sender) : Controller
    {

        // Trasa do akcji będzie 'api/Users/employee/{employeeEmail}'
        [HttpGet("employee/{employeeEmail}")]
        [ProducesResponseType(200, Type = typeof(EmployeeDTO))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployee(string employeeEmail)
        {
            var serviceResult = await sender.Send(new GetEmployeeByEmailQuerry(employeeEmail));

            if (!serviceResult.Succeed) return NotFound();

            return Ok(serviceResult.Data);
         }

        [HttpGet("employer/{employerEmail}")]
        [ProducesResponseType(200, Type = typeof(EmployerDTO))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployer(string employerEmail)
        {
            var serviceResult = await sender.Send(new GetEmployerByEmailQuerry(employerEmail));

            if (!serviceResult.Succeed) return NotFound();

            return Ok(serviceResult.Data);
        }

        [HttpGet("yourEmployees/{currentUserEmail}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeDTO>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Employer")]
        public async Task<IActionResult> GetYourEmployees(string currentUserEmail)
        {
            var serviceResult = await sender.Send(new GetYourEmployeesQuerry(currentUserEmail));

            if (!serviceResult.Succeed)
                return BadRequest();

            return Ok(serviceResult.Data);
        }

    }
}
