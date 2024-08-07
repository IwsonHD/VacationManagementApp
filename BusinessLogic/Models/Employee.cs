using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Employee : User
    {

        
        [DataType(DataType.EmailAddress)]
        public string EmployersEmail {  get; set; }
        public ICollection<Vacation> Vacations { get; set; }
        public bool EmployeeConfirmed { get; set; } 
        
    }


}
