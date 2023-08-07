using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationManagementApp.DataBases;
using VacationManagementApp.Models;

namespace VacationManagementApp.Controllers
{
    public class VacationsController : Controller
    {

        private readonly VacationManagerDbContext _db;

        public VacationsController(VacationManagerDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Vacation> userVacations = _db.Vacations
                .Where(v => v.EmployeeId == currUserId)
                .ToList();


            return View(userVacations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vacation vacation)
        {
            vacation.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            
            
                Vacation newVacation = new Vacation
                {
                    HowManyDays = vacation.HowManyDays,
                    EmployeeId = vacation.EmployeeId,
                    When = vacation.When,
                    state = Enums.VacationState.Waiting
                };


                await _db.Vacations.AddAsync(newVacation);
                await _db.SaveChangesAsync();


                return RedirectToAction("Index");


        
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult YourEmployeesVacation(string email)
        {
            Employee? employee = _db.Employees.FirstOrDefault(e => e.Email == email);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<Vacation> employeesVacation = _db.Vacations
                    .Where(v => v.EmployeeId == employee.Id)
                    .ToList();


                return View(employeesVacation);
            }

        }

        [HttpGet]
        public IActionResult EditState(int vacationsId)
        {
            if (vacationsId == null || vacationsId == 0)
            {
                return NotFound();
            }

            var vacationFromDb = _db.Vacations.Find(vacationsId);

            if(vacationFromDb == null)
            {
                return NotFound();
            }

            return View(vacationFromDb);
        }        

       


    }
}
