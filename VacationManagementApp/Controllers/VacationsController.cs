using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BusinessLogic.DataBasesContext;
using BusinessLogic.Models;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace VacationManagementApp.Controllers
{
    public class VacationsController : Controller
    {

        private readonly VacationManagerDbContext _db;
        private readonly IVacationService _vacationService;

        public VacationsController(VacationManagerDbContext db, IVacationService vacationService)
        {
            _db = db;
            _vacationService = vacationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //IEnumerable<Vacation> userVacations = _db.Vacations
            //    .Where(v => v.EmployeeId == currUserId)
            //    .ToList();


            return View(_vacationService.GetVacations());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacationDto vacation)
        {
            if(!ModelState.IsValid) return View(vacation);

            var serviceResult = await _vacationService.AddVacationToDb(vacation);

            if (!serviceResult.Succeed)
            {
                serviceResult.UpdateModelError(ModelState);
                return View(vacation);
            }

            return RedirectToAction("Index");
        
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult YourEmployeesVacation(string email)
        {

            return View(_vacationService.GetYoursEmployeeVacation(email));






            //Employee? employee = _db.Employees.FirstOrDefault(e => e.Email == email);

            //if (employee == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    IEnumerable<Vacation> employeesVacation = _db.Vacations
            //        .Where(v => v.EmployeeId == employee.Id)
            //        .ToList();


            //    return View(employeesVacation);
            //}

        }

        [HttpGet]
        public IActionResult EditState(int? Id)
        {
            var vacationsFromDb = _vacationService.GetVacation(Id);
            if (vacationsFromDb == null)
            {
                return NotFound();
            }
            return View(vacationsFromDb);



            //if (Id == null || Id == 0)
            //{
            //    return NotFound();
            //}

            //var vacationFromDb = _db.Vacations.Find(Id);

            //if(vacationFromDb == null)
            //{
            //    return NotFound();
            //}

            //return View(vacationFromDb);
        }


        [HttpPost]
        public IActionResult EditState(Vacation editedVacation)
        {
            var serviceResult = _vacationService.EditVacation(editedVacation);

            if(!serviceResult.Succeed)
            {
                serviceResult.UpdateModelError(ModelState);
                return View(editedVacation);
            }

            TempData["success"] = "State has been successfully updated";
            return RedirectToAction("YourEmployeesVacation", "Vacations", new { serviceResult.Data });



            //ModelState.Remove("Employee");


            //if (ModelState.IsValid)
            //{
            //    string Email = _db.Employees.SingleOrDefault(e => e.Id == editedVacation.EmployeeId).Email;
            //    if(Email == null) { return NotFound(); }

            //    _db.Vacations.Update(editedVacation);
            //    _db.SaveChanges();
            //    TempData["success"] = "State has been successfully updated";
            //    return RedirectToAction("YourEmployeesVacation","Vacations",new { Email });

            //}

            //return View(editedVacation);
        }
       


    }
}
