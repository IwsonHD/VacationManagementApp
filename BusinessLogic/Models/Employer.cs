using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Employer : User
    {

        public string CompanyName { get; set; }
    }
}
