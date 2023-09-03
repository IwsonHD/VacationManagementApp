using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacationManagementApp.DataBases;
using VacationManagementApp.Models;
using VacationManagementApp.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace VacationManagementApp.Controllers
{
    public class VacationsController : Controller
    {

        
        private readonly IVacationService _vacationService;

        public VacationsController(IVacationService vacationService)
        {
            
            _vacationService = vacationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vacations = _vacationService.GetVacations(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(vacations == null)
            {
                return RedirectToAction("EmployeeNotConfirmed","Account");
            }

			return View(vacations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vacation vacation)
        {
            ModelState.Remove("Employee");
            ModelState.Remove("EmployeeId");
            vacation.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _vacationService.AddVacationToDb(vacation);
                return RedirectToAction("Index");
            }


            return View(vacation);
        
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult YourEmployeesVacation(string email)
        {

            return View(_vacationService.GetYoursEmployeeVacation(email));

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

        }


        [HttpPost]
        public IActionResult EditState(Vacation editedVacation)
        {
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                string email = _vacationService.EditVacation(editedVacation);
               if (email != null)
                {
                    TempData["success"] = "State has been successfully updated";
                    return RedirectToAction("YourEmployeesVacation", "Vacations", new { email });
                }
                return View(editedVacation);
            }
            return View(editedVacation);







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
