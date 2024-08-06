using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VacationManagementApp.Interfaces;
using VacationManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VacationManagementApp.Dto;
using VacationManagementApp.AssistanceClasses;

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

        public async Task<ServiceResult<String?>> LoginUser(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var serviceResult = new ServiceResult<String?>();

            if (!user.EmailConfirmed)
            {
                serviceResult.AppendError(nameof(LoginDto.Email), "Confirm your e-mail to log in");
            }
 
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
 
            if (!result.Succeeded)
            {
                serviceResult.AppendError(String.Empty, "Wrong password or email");
            }
            return serviceResult;
            
        }

        public async Task<ServiceResult<bool>> AcceptNewUser(string newUsersEmail)
        {
            var user = await _userManager.FindByEmailAsync(newUsersEmail);
            var serviceResult = new ServiceResult<bool>();

            if (user == null)
            {
                serviceResult.AppendError(string.Empty, "Such user does not exist");
                return serviceResult;
            }

            if (user is not Employee employee || !await _userManager.IsInRoleAsync(user, "Employee"))
            {
                serviceResult.AppendError(string.Empty, "This user is not an employee");
                return serviceResult;

            }
            else
            {
                employee.EmployeeConfirmed = true;
                var updateResult = await _userManager.UpdateAsync(employee);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        serviceResult.AppendError(string.Empty, error.Description);
                    }
                }
            }
            return serviceResult;
        }







        public async Task<ServiceResult<bool>> RegisterUser(RegisterDto model)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>();



            
            if (!await _roleManager.RoleExistsAsync(model.Role)) 
                serviceResult.AppendError(nameof(model.Role), "Choosen role does not exist");
                
            

            if (null != await _userManager.FindByEmailAsync(model.Email)) 
                serviceResult.AppendError(nameof(model.Email), "This e-mail address is allready taken");
                
            

            //Append more checks for example phone uniqness check etc.
            
            if(!serviceResult.Succeed) 
                return serviceResult;
            
            User newUser;

            if(model.Role == "Employee")
            {
                var doesEmployeeExist = await _userManager.FindByEmailAsync(model.EmployersEmail);
                
                if(await _userManager.IsInRoleAsync(doesEmployeeExist, "Employee"))
                    serviceResult.AppendError(nameof(model.EmployersEmail), "There is no Employer with such email");
                newUser = new Employee
                {
                    UserName = model.Email,
                    //EmailAddress = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    EmployersEmail = model.EmployersEmail,
                    EmployeeConfirmed = false
                };
                
             
            }else{
                
                
                newUser = new Employer
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    CompanyName = model.CompanyName
                };
            }

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    serviceResult.AppendError(String.Empty, error.Description);
                }
                return serviceResult;
            }

            result = await _userManager.AddToRoleAsync(newUser, model.Role);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    serviceResult.AppendError(String.Empty, error.Description);
                }
                return serviceResult;
            }

            return serviceResult;

















            //if (model.Role == "Employee")
            //{
            //    var doesEmployeeExist = await _userManager.FindByEmailAsync(model.EmployersEmail);

            //    if (doesEmployeeExist == null || await _userManager.IsInRoleAsync(doesEmployeeExist, "Employee"))
            //    {
            //        _actionContextAccessor.ActionContext.ModelState.AddModelError("EmployersEmail", "There is no Employer with such email");
            //        return false;
            //    }

            //    newUser = new Employee
            //    {
            //        UserName = model.Email,
            //        EmailAddress = model.Email,
            //        FirstName = model.FirstName,
            //        LastName = model.LastName,
            //        PhoneNumber = model.PhoneNumber,
            //        Email = model.Email,
            //        EmployersEmail = model.EmployersEmail
            //    };
            //}
            //else
            //{
            //    newUser = new Employer
            //    {
            //        UserName = model.Email,
            //        FirstName = model.FirstName,
            //        EmailAddress = model.Email,
            //        LastName = model.LastName,
            //        PhoneNumber = model.PhoneNumber,
            //        Email = model.Email,
            //        CompanyName = model.CompanyName
            //    };
            //}

            //var result = await _userManager.CreateAsync(newUser, model.Password);

            //if (result.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(newUser, model.Role);
            //    //await _signInManager.SignInAsync(newUser, isPersistent: false);
            //    return true;
            //}
            //foreach (var error in result.Errors)
            //{
            //    _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, error.Description);
            //}

            //return false;

        }

    }
}
