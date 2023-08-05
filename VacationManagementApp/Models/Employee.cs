using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VacationManagementApp.Models
{
    public class Employee : User
    {

        
        [DataType(DataType.EmailAddress)]
        public string EmployersEmail {  get; set; }
        public ICollection<Vacation> Vacations { get; set; }
        
    }
}
