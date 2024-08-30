using BusinessLogic.AssistanceClasses;
using BusinessLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CQRS.Commands.AddVacation
{
    public record AddVacationCommand(VacationDto vacationDto, string userId) : IRequest<ServiceResult<bool>>;

}
