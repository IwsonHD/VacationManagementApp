using BusinessLogic.AssistanceClasses;
using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface IHomeService
    {
        Task<User>? ShowYourData();

        Task<IEnumerable<Employee>>? ShowYourEmployees();

        Task<ServiceResult<IEnumerable<Employee>>>? ShowNewEmployees();

    }
}
