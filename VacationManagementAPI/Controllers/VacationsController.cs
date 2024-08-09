using BusinessLogic.DTOs;
using BusinessLogic.Models;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public IActionResult GetEmployeeVacations(string employeeEmail)
        {
            var employeeVacations = _vacationService.GetYoursEmployeeVacation(employeeEmail);

            if (employeeVacations.IsNullOrEmpty()) return NotFound();

            return Ok(employeeVacations);
        }
    }
}
