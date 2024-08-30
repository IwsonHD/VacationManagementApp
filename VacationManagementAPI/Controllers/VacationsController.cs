using BusinessLogic.DTOs;
using BusinessLogic.Models;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.CQRS.Queries.GetVacations;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using BusinessLogic.CQRS.Commands.AddVacation;


namespace VacationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class VacationsController(ISender sender, IHttpContextAccessor contextAccessor) : Controller
    { 
        
        [HttpGet("{employeeEmail}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Vacation>))]
        [ProducesResponseType(404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployeeVacations(string employeeEmail)
        {
            var serviceResult = await sender.Send(new GetVacationsByEmployeeEmailQuerry(employeeEmail));

            if (!serviceResult.Succeed) 
                return NotFound();

            return Ok(serviceResult.Data);
        }

        [HttpPost("add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Employee")]
        public async Task<IActionResult> AddVacation([FromBody] VacationDto vacationDto)
        {
            var userId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (String.IsNullOrEmpty(userId))
                return BadRequest();

            var serviceResult = await sender.Send(new AddVacationCommand(vacationDto, userId));

            if (!serviceResult.Succeed)
                return BadRequest();

            return Ok();

        }

    }
}
