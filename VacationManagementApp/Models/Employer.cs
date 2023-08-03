using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VacationManagementApp.Models
{
    public class Employer : User
    {

        public string CompanyName { get; set; }
    }
}
