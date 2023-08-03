using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VacationManagementApp.Models
{
    public class Employee : User
    {

        [Required]
        public int? EmployersID {  get; set; }
        
    }
}
