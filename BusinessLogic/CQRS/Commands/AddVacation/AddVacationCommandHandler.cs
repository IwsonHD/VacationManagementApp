using BusinessLogic.AssistanceClasses;
using BusinessLogic.DataBasesContext;
using BusinessLogic.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CQRS.Commands.AddVacation
{
    public class AddVacationCommandHandler(
        VacationManagerDbContext db
        ) : 
        IRequestHandler<AddVacationCommand, ServiceResult<bool>>
    {
        public async Task<ServiceResult<bool>> Handle(AddVacationCommand request, CancellationToken cancellationToken)
        {
            var serviceResult = new ServiceResult<bool>();
            

            var vacation = new Vacation
            {
                HowManyDays = request.vacationDto.HowManyDays,
                When = request.vacationDto.When,
                EmployeeId = request.userId
            };

            try
            {
                await db.Vacations.AddAsync(vacation);
                await db.SaveChangesAsync();

            }catch( OperationCanceledException ex)
            {
                //to be changed depending wheter the api is supposed to the same as app
                serviceResult.AppendError(string.Empty, "User was not an employee");
            }

            return serviceResult;
        }
    }
}
