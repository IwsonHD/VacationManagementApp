﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacationManagementApp.DataBases;
using VacationManagementApp.Models;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Dto;

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

            if(await _vacationService.AddVacationToDb(vacation))
            {
                return RedirectToAction("Index");
            }
            return View(vacation);








            //vacation.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            //if (ModelState.IsValid)
            //{

            //    Vacation newVacation = new Vacation
            //    {
            //        HowManyDays = vacation.HowManyDays,
            //        EmployeeId = vacation.EmployeeId,
            //        When = vacation.When,
            //        state = Enums.VacationState.Waiting
            //    };


            //    await _db.Vacations.AddAsync(newVacation);
            //    await _db.SaveChangesAsync();


            //    return RedirectToAction("Index");
            //}

            //return View(vacation);
        
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
            string email = _vacationService.EditVacation(editedVacation);

            if(email != null)
            {
                TempData["success"] = "State has been successfully updated";
                return RedirectToAction("YourEmployeesVacation","Vacations",new { email });
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
