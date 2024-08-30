using BusinessLogic.AssistanceClasses;
using BusinessLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CQRS.Queries.GetVacations
{
    public record GetVacationsByEmployeeEmailQuerry(string employeeEmail) : IRequest<ServiceResult<IEnumerable<VacationDto>>>;
}
