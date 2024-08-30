using AutoMapper;
using BusinessLogic.AssistanceClasses;
using BusinessLogic.CQRS.Queries.GetEmployee;
using BusinessLogic.DataBasesContext;
using BusinessLogic.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CQRS.Queries.GetEmployer
{
    public class GetEmployeeByEmailQuerryHandler(VacationManagerDbContext db, IMapper mapper) : IRequestHandler<GetEmployeeByEmailQuerry, ServiceResult<EmployeeDTO>>
    {
        public async Task<ServiceResult<EmployeeDTO>> Handle(GetEmployeeByEmailQuerry request, CancellationToken cancellationToken)
        {
            ServiceResult<EmployeeDTO> serviceResult = new();
            
            var employee = await db.Employees.FirstOrDefaultAsync(emp => emp.Email == request.employeeEmail);
            
            if(employee == null)
                serviceResult.AppendError(string.Empty, "Such employer does not exist");

            var employeeDTO = mapper.Map<EmployeeDTO>(employee);

            serviceResult.Data = employeeDTO;

            return serviceResult;
        }
    }
}
