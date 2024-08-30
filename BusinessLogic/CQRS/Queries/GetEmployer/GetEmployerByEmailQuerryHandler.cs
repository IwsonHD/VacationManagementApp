using BusinessLogic.AssistanceClasses;
using BusinessLogic.DataBasesContext;
using BusinessLogic.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogic.CQRS.Queries.GetEmployer;

public class GetEmployerByEmailQuerryHandler(VacationManagerDbContext db) : IRequestHandler<GetEmployerByEmailQuerry, ServiceResult<EmployerDTO>>
{

    public async Task<ServiceResult<EmployerDTO>> Handle(GetEmployerByEmailQuerry request, CancellationToken cancellationToken)
    {
        ServiceResult<EmployerDTO> serviceResult = new();
        var employer = await db.Employers.FirstOrDefaultAsync(emp => emp.Email == request.employerEmail);

        if (employer == null)
            serviceResult.AppendError(string.Empty, "Such employer does not exist");

        var employerDTO = new EmployerDTO
        {
            Email = employer.Email,
            CompanyName = employer.CompanyName,
            PhoneNumber = employer.PhoneNumber,
            Role = "Employer",
            FirstName = employer.FirstName,
            LastName = employer.LastName
        };

        serviceResult.Data = employerDTO;

        return serviceResult;
    }
}

