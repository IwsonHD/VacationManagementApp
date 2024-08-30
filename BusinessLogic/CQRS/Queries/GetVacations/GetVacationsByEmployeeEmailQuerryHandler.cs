using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.AssistanceClasses;
using BusinessLogic.DataBasesContext;
using BusinessLogic.DTOs;
using BusinessLogic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.CQRS.Queries.GetVacations
{
    public class GetVacationsByEmployeeEmailQuerryHandler(VacationManagerDbContext db)
        : IRequestHandler<GetVacationsByEmployeeEmailQuerry, ServiceResult<IEnumerable<VacationDto>>>
    {
        public async Task<ServiceResult<IEnumerable<VacationDto>>> Handle(GetVacationsByEmployeeEmailQuerry request, CancellationToken cancellationToken)
        {
            ServiceResult<IEnumerable<VacationDto>> serviceResult = new();
            Employee? employee = await db.Employees.FirstOrDefaultAsync(emp => emp.Email == request.employeeEmail);

            if (employee == null)
            {
                serviceResult.AppendError(String.Empty, "No such employee exists");
                return serviceResult;
            }

            var employeeVacations = db.Vacations
                .Where(vac => vac.EmployeeId == employee.Id)
                .ToListAsync();
            
            var employeeVacationsDto = new List<VacationDto>();

            foreach (var vac in await employeeVacations)
            {
                var vacDto = new VacationDto
                {
                    HowManyDays = vac.HowManyDays,
                    When = vac.When
                };
                employeeVacationsDto.Add(vacDto);
            }

            serviceResult.Data = employeeVacationsDto;

            return serviceResult;
        }


    }
}
