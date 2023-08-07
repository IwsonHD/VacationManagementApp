using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VacationManagementApp.Dto;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;

namespace VacationManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager
            ,IAccountService userService)
          
        {
            _accountService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            //_userService.AddUser();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {

            if(!await _accountService.LoginUser(model))
            {
                return View(model);
            }
            return RedirectToAction("Index", "home");






            //if (ModelState.IsValid)
            //{
            //    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }

            //    ModelState.AddModelError(string.Empty, "Wrong password or login");            
            
            //}
            //return View(model);
            
        }





        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterDto model)
        {
            if(!await _accountService.RegisterUser(model))
            {
                return View(model);
            }
            
            return RedirectToAction("Index", "Home");




            //if(ModelState.IsValid)
            //{
            //    if(!await _roleManager.RoleExistsAsync(model.Role))
            //    {
            //        return View(model);
            //    }

            //    bool does_exist = null != await _userManager.FindByEmailAsync(model.Email);

            //    if (does_exist)
            //    {
            //        ModelState.AddModelError("Email", "This e-mail address is allready taken");
            //        return View(model);
            //    }

            //    User newUser;
            //    if(model.Role == "Employee")
            //    {
            //        var doesEmployerExist = await _userManager.FindByEmailAsync(model.EmployersEmail);
            //        if (doesEmployerExist == null || await _userManager.IsInRoleAsync(doesEmployerExist, "Employee"))
            //        {
            //            ModelState.AddModelError("EmployersEmail", "There is no Employer with such email");
            //            return View(model);
            //        }

            //        newUser = new Employee
            //        {
            //            UserName = model.Email,
            //            EmailAddress = model.Email,
            //            FirstName = model.FirstName,
            //            LastName = model.LastName,
            //            PhoneNumber = model.PhoneNumber,
            //            Email = model.Email,
            //            EmployersEmail = model.EmployersEmail
            //        };

            //    }else
            //    {
            //        newUser = new Employer
            //        {
            //            UserName = model.Email,
            //            FirstName = model.FirstName,
            //            EmailAddress = model.Email,
            //            LastName = model.LastName,
            //            PhoneNumber = model.PhoneNumber,
            //            Email = model.Email,
            //            CompanyName = model.CompanyName
            //        };
            //    }


            //    var result = await _userManager.CreateAsync(newUser, model.Password);

            //    if(result.Succeeded)
            //    {
            //        await _userManager.AddToRoleAsync(newUser, model.Role);
            //        await _signInManager.SignInAsync(newUser, isPersistent: false);

            //        return RedirectToAction("Index", "Home");
            //    }
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }


            //}
            //return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
    
}
