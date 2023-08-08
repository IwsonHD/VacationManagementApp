using VacationManagementApp.Models;

namespace VacationManagementApp.Interfaces
{
    public interface IHomeService
    {
        Task<User>? ShowYourData();

        Task<IEnumerable<Employee>>? ShowYourEmployees();

    }
}
