using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VacationManagementApp.Dto;

namespace VacationManagementApp.Services
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IActionContextAccessor _actionContextAccessor;

        public AccountService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            IActionContextAccessor actionContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<bool> LoginUser(LoginDto model)
        {
            if(_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return true;
                }
                _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Wrong password or login");
                return false;
            }
            return false;
        }








        public async Task<bool> RegisterUser(RegisterDto model)
        {



            if (_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(model.Role)) return false;


                bool does_exist = null != await _userManager.FindByEmailAsync(model.Email);

                if (does_exist)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError("Email", "This e-mail address is allready taken");
                    return false;
                }

                User newUser;

                if (model.Role == "Employee")
                {
                    var doesEmployeeExist = await _userManager.FindByEmailAsync(model.EmployersEmail);

                    if (doesEmployeeExist == null || await _userManager.IsInRoleAsync(doesEmployeeExist, "Employee"))
                    {
                        _actionContextAccessor.ActionContext.ModelState.AddModelError("EmployersEmail", "There is no Employer with such email");
                        return false;
                    }

                    newUser = new Employee
                    {
                        UserName = model.Email,
                        EmailAddress = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        EmployersEmail = model.EmployersEmail
                    };
                }
                else
                {
                    newUser = new Employer
                    {
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        EmailAddress = model.Email,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        CompanyName = model.CompanyName
                    };
                }

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, model.Role);
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return true;
                }
                foreach (var error in result.Errors)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, error.Description);
                }



                return false;
            }
            return false;
        }

    }
}
