using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Encodings.Web;
using VacationManagementApp.Dto;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;
using VacationManagementApp.Validators;

namespace VacationManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
      //  private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailServices _emailServices;

        public AccountController(
            UserManager<User> userManager,
            //RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            IEmailServices emailServices
            ,IAccountService userService)
          
        {
            _emailServices = emailServices;
            _accountService = userService;
          //  _userManager = userManager;
            //_roleManager = roleManager;
            _signInManager = signInManager;

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if(!ModelState.IsValid) return View(model);

            var serviceResult = await _accountService.LoginUser(model);

            if (!serviceResult.Succeed)
            {
                serviceResult.ManageModelState(ModelState);
                return View(model);
            }

            return RedirectToAction("Index", "home");





            //if(!await _accountService.LoginUser(model))
            //{
            //    return View(model);
            //}
            //return RedirectToAction("Index", "home");
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
            if (!ModelState.IsValid) {
                return View(model);            
            }
            

            var serviceResult = await _accountService.RegisterUser(model);
            serviceResult.ManageModelState(ModelState);

            if (!serviceResult.Succeed)
            {
                return View(model);
            }

            var token = serviceResult.GetResult("token");
            var userId = serviceResult.GetResult("userId");

            if (token == null || userId == null) {
                return View("Error");
            }

            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, token }, Request.Scheme);

            await _emailServices.SendEmail(model.Email, "Confirm your e-mail",
                 $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            
            //if(serviceResult.GetResult("isNewEmployee") == "true")
            //{
            //    var callbackUrlForEmployeeConfirmation = Url.Action("ConfirmEmployee", "Account", new {employeeId},Request.Scheme);
            //}


            return RedirectToAction("PostRegister");

            //if(!await _accountService.RegisterUser(model))
            //{
            //    return View(model);
            //}

            //return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult PostRegister() 
        {
            return View();        
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(! await _emailServices.ConfirmEmail(userId, token))
            {
                return View("Error");
            }

            TempData["success"] = "Email has been succeesfully confirmed";
            return RedirectToAction("Index", "Home");
        }

        

    }
    
}
