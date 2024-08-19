using BusinessLogic.DTOs;
using BusinessLogic.Models;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace VacationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class VacationsController : Controller
    {
        private readonly IVacationService _vacationService;

        public VacationsController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        
        [HttpGet("{employeeEmail}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Vacation>))]
        [ProducesResponseType(404)]
        [Authorize]
        public IActionResult GetEmployeeVacations(string employeeEmail)
        {
            var serviceResult = _vacationService.GetYoursEmployeeVacation(employeeEmail);

            if (!serviceResult.Succeed) return NotFound();

            return Ok(serviceResult.Data);
        }
    }
}
