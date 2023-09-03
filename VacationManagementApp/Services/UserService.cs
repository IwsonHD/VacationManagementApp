using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VacationManagementApp.Dto;
using VacationManagementApp.Validators;
using System.Security.Policy;
using VacationManagementApp.DataBases;
using Microsoft.EntityFrameworkCore;

namespace VacationManagementApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly VacationManagerDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IActionContextAccessor _actionContextAccessor;

        public AccountService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            VacationManagerDbContext db
            /*IActionContextAccessor actionContextAccessor*/)
        {
            _db = db;
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
            else if(result.IsNotAllowed)
            {
                serviceResult.AddError(string.Empty, "Confirm your e-mail to access your account");
            }
            else
            {
                serviceResult.AddError(string.Empty, "Wrong password or login!");
            }

            
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







            Guid? employeeToken = null;
            User newUser;

            if(model.Role == "Employee")
            {
                var doesEmployeeExist = await _userManager.FindByEmailAsync(model.EmployersEmail);

                if (doesEmployeeExist == null || await _userManager.IsInRoleAsync(doesEmployeeExist, "Employee"))
                {
                    serviceResult.AddError(nameof(model.EmployersEmail), "There is no Employer with such email");
                    return serviceResult;
                }
                employeeToken = Guid.NewGuid();

                newUser = new Employee()
                {
                    UserName = model.Email,
                    EmailAddress = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    EmployersEmail = model.EmployersEmail,
                    Token = employeeToken.ToString()
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
                //await _signInManager.SignInAsync(newUser, isPersistent: false);

                var userId = newUser.Id;
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                serviceResult.AddResult(nameof(token), token);
                serviceResult.AddResult(nameof(userId), userId);

                if(model.Role == "Employee")
                {
                    serviceResult.AddResult(nameof(employeeToken), employeeToken.ToString());
                    serviceResult.AddResult("isNewEmployee", "true");
                }
                else
                {
                    serviceResult.AddResult(nameof(employeeToken), employeeToken.ToString());
                    serviceResult.AddResult("isNewEmployee", "false");
                }

                return serviceResult;

            }
            foreach (var error in result.Errors)
            {
                serviceResult.AddError(string.Empty, error.Description);
            }
            return serviceResult;


        }


        public async Task<bool> ConfirmEmployee(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return false;
            }

            var user = await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == userId);

            if (user == null)
            {
                return false;
            }

            if(user.Token != token)
            {
                return false;
            }


            user.IsConfirmed = true;
            _db.Employees.Update(user);
            await _db.SaveChangesAsync();
            return true;


        }


    }
}
