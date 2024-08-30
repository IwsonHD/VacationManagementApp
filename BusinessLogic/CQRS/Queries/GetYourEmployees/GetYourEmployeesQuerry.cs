using BusinessLogic.AssistanceClasses;
using BusinessLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CQRS.Queries.GetYourEmployees
{
    public record GetYourEmployeesQuerry(string yourEmail)
        : IRequest<ServiceResult<IEnumerable<EmployeeDTO>>>;
}
