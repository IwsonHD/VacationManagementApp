using AutoMapper;
using BusinessLogic.AssistanceClasses;
using BusinessLogic.DTOs;
using BusinessLogic.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CQRS.Queries.GetYourEmployees
{
    public class GetYourEmployeesQuerryHandler(UserManager<User> userManager,
        IMapper mapper)
        : IRequestHandler<GetYourEmployeesQuerry, ServiceResult<IEnumerable<EmployeeDTO>>>
    {
        public async Task<ServiceResult<IEnumerable<EmployeeDTO>>> Handle(GetYourEmployeesQuerry request, CancellationToken cancellationToken)
        {
            var serviceResult = new ServiceResult<IEnumerable<EmployeeDTO>>();
            
            var currentUser = await userManager.FindByEmailAsync(request.yourEmail);

            if (currentUser == null)
            {
                serviceResult.AppendError(String.Empty, "Employer with given address does not exist");
                return serviceResult;
            }

            var currUserEmployees = userManager.Users
                .OfType<Employee>()
                .Where(e => e.EmployersEmail == currentUser.Email)
                //.Where(e => e.EmployeeConfirmed)
                .AsQueryable();

            var employeesOut = mapper.Map<List<EmployeeDTO>>(currUserEmployees.ToList());

            serviceResult.Data = employeesOut;  

            return serviceResult;
        }
    }
}
