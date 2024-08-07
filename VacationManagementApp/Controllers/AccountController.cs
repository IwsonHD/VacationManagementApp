using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services;

namespace VacationManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
      //  private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<User> userManager,
            //RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            IAccountService userService,
            IEmailService emailService)
          
        {
            _accountService = userService;
            _userManager = userManager;
            _emailService = emailService;
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
            var result = await _accountService.LoginUser(model);
            if (!result.Succeed)
            {
                result.UpdateModelError(ModelState);
                return View(model);
            }

            TempData["success"] = "Loged in successfully";
            return RedirectToAction("Index", "home");
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
            var serviceResult = await _accountService.RegisterUser(model);
            if(!serviceResult.Succeed)
            {
                serviceResult.UpdateModelError(ModelState);
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("ConfirmEmail",
                "Account",
                new { userId = user.Id, code = code },
                protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmailConfirmationAsync(model.Email, "Confirm your email",
                $"Please confirm your account via <a href='{callbackUrl}'>clicking here</a>.");

            return RedirectToAction("PostRegister");

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(! await _emailService.ConfirmaEmailAsync(userId, code))
            {
                return View("Error");
            }
            TempData["Success"] = "Email has been succesfully confirmed";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AcceptNewEmployee(string email) 
        { 
            var serviceResult = await _accountService.AcceptNewUser(email);
            if (!serviceResult.Succeed)
            {
                return View("Error");
            }

            return RedirectToAction("YourEmployees", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PostRegister()
        {
            return View();
        }
    }
    
}
