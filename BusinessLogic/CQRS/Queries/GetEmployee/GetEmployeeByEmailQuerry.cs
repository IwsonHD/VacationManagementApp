using BusinessLogic.AssistanceClasses;
using MediatR;
using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CQRS.Queries.GetEmployee
{
    public record GetEmployeeByEmailQuerry(string employeeEmail) : IRequest<ServiceResult<EmployeeDTO>>;
   
}
