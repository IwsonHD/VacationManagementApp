using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VacationManagementApp.Dto;
using VacationManagementApp.Validators;

namespace VacationManagementApp.Services
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IActionContextAccessor _actionContextAccessor;

        public AccountService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager
            /*IActionContextAccessor actionContextAccessor*/)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            //_actionContextAccessor = actionContextAccessor;
        }
        //tutaj tez validacja w kotrolerze
        public async Task<ServiceResult> LoginUser(LoginDto model)
        {
            var serviceResult = new ServiceResult();

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return serviceResult;
            }

            serviceResult.AddError(string.Empty, "Wrong password or login!");
            return serviceResult;

            
            //if(_actionContextAccessor.ActionContext.ModelState.IsValid)
            //{
            //    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            //    if (result.Succeeded)
            //    {
            //        return true;
            //    }
            //    _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Wrong password or login");
            //    return false;
            //}
            //return false;
        }







        //zwaliduj w kontrolerze za pomoca validatora a tutaj tylko dodam do bazy danch
        //dzieki czemu bede mial pewnosc ze wszystko z modelem jest okej
        public async Task<ServiceResult> RegisterUser(RegisterDto model)
        {
            var serviceResult = new ServiceResult();
            
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                serviceResult.AddError(model.Role, "Such role does not exist");
                return serviceResult;
            }

            bool does_exist = null != await _userManager.FindByEmailAsync(model.Email);
            
            if (does_exist)
            {
                serviceResult.AddError(nameof(model.Email), "This e-mail address is allready taken");
                return serviceResult;
            }








            User newUser;

            if(model.Role == "Employee")
            {
                var doesEmployeeExist = await _userManager.FindByEmailAsync(model.EmployersEmail);

                if (doesEmployeeExist == null || await _userManager.IsInRoleAsync(doesEmployeeExist, "Employee"))
                {
                    serviceResult.AddError(nameof(model.EmployersEmail), "There is no Employer with such email");
                    return serviceResult;
                }

                newUser = new Employee()
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
                if(null !=_userManager.Users
                    .OfType<Employer>()
                    .FirstOrDefault(e=>e.CompanyName == model.CompanyName)
                    )
                {
                    serviceResult.AddError(nameof(model.CompanyName), "There allready exists an employer with such comapny name!");
                    return serviceResult;
                }





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

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, model.Role);
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return serviceResult;

            }
            foreach (var error in result.Errors)
            {
                serviceResult.AddError(string.Empty, error.Description);
            }
            return serviceResult;




                //var nzw = nameof (LoginDto.Email)
                //if (_actionContextAccessor.ActionContext.ModelState.IsValid)
                //{
                //    if (!await _roleManager.RoleExistsAsync(model.Role)) return false;


                //    bool does_exist = null != await _userManager.FindByEmailAsync(model.Email);

                //    if (does_exist)
                //    {
                //        _actionContextAccessor.ActionContext.ModelState.AddModelError("Email", "This e-mail address is allready taken");
                //        return false;
                //    }

                //    User newUser;

                //    if (model.Role == "Employee")
                //    {
                //        var doesEmployeeExist = await _userManager.FindByEmailAsync(model.EmployersEmail);

                //        if (doesEmployeeExist == null || await _userManager.IsInRoleAsync(doesEmployeeExist, "Employee"))
                //        {
                //            _actionContextAccessor.ActionContext.ModelState.AddModelError("EmployersEmail", "There is no Employer with such email");
                //            return false;
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
                //    }
                //    else
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

                //    if (result.Succeeded)
                //    {
                //        await _userManager.AddToRoleAsync(newUser, model.Role);
                //        await _signInManager.SignInAsync(newUser, isPersistent: false);
                //        return true;
                //    }
                //    foreach (var error in result.Errors)
                //    {
                //        _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, error.Description);
                //    }



                //    return false;
                //}
                //return false;
        }

    }
}
