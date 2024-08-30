using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BusinessLogic.DataBasesContext;
using BusinessLogic.Models;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.CQRS.Queries.GetVacations;
using MediatR;
using BusinessLogic.Services;

namespace VacationManagementApp.Controllers
{
    public class VacationsController(ISender sender,VacationManagerDbContext db, IVacationService vacationService) : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            //var currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //IEnumerable<Vacation> userVacations = db.Vacations
            //    .Where(v => v.EmployeeId == currUserId)
            //    .ToList();
            var serviceResult = vacationService.GetVacations();

            if(!serviceResult.Succeed)
                return NotFound();

            return View(serviceResult.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacationDto vacation)
        {
            if(!ModelState.IsValid) return View(vacation);

            var serviceResult = await vacationService.AddVacationToDb(vacation);

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
            var serviceResult = vacationService.GetYoursEmployeeVacation(email);

            if (!serviceResult.Succeed)
            {
                return NotFound();
            }


            return View(serviceResult.Data);






            //Employee? employee = db.Employees.FirstOrDefault(e => e.Email == email);

            //if (employee == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    IEnumerable<Vacation> employeesVacation = db.Vacations
            //        .Where(v => v.EmployeeId == employee.Id)
            //        .ToList();


            //    return View(employeesVacation);
            //}

        }

        [HttpGet]
        public IActionResult EditState(int? Id)
        {
            var serviceResult = vacationService.GetVacation(Id);
            if (!serviceResult.Succeed)
            {
                return NotFound();
            }
            return View(serviceResult.Data);



            //if (Id == null || Id == 0)
            //{
            //    return NotFound();
            //}

            //var vacationFromDb = db.Vacations.Find(Id);

            //if(vacationFromDb == null)
            //{
            //    return NotFound();
            //}

            //return View(vacationFromDb);
        }


        [HttpPost]
        public IActionResult EditState(Vacation editedVacation)
        {
            var serviceResult = vacationService.EditVacation(editedVacation);

            if(!serviceResult.Succeed)
            {
                serviceResult.UpdateModelError(ModelState);
                return View(editedVacation);
            }

            TempData["success"] = "State has been successfully updated";
            
            var email = serviceResult.Data;

            return RedirectToAction("YourEmployeesVacation", "Vacations", new { email });



            //ModelState.Remove("Employee");


            //if (ModelState.IsValid)
            //{
            //    string Email = db.Employees.SingleOrDefault(e => e.Id == editedVacation.EmployeeId).Email;
            //    if(Email == null) { return NotFound(); }

            //    db.Vacations.Update(editedVacation);
            //    db.SaveChanges();
            //    TempData["success"] = "State has been successfully updated";
            //    return RedirectToAction("YourEmployeesVacation","Vacations",new { Email });

            //}

            //return View(editedVacation);
        }
       


    }
}
