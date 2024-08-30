using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.AssistanceClasses;
using BusinessLogic.DTOs;
using MediatR;

namespace BusinessLogic.CQRS.Queries.GetEmployer
{
    public record GetEmployerByEmailQuerry(string employerEmail) : IRequest<ServiceResult<EmployerDTO>>;
}
