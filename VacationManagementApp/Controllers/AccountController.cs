using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VacationManagementApp.Dto;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;
using VacationManagementApp.Validators;

namespace VacationManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        //private readonly UserManager<User> _userManager;
      //  private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            //UserManager<User> userManager,
            //RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager
            ,IAccountService userService)
          
        {
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

            

            return RedirectToAction("Index", "Home");

            //if(!await _accountService.RegisterUser(model))
            //{
            //    return View(model);
            //}

            //return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
    
}
