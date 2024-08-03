using System.ComponentModel.DataAnnotations;
using VacationManagementApp.Validation;

namespace VacationManagementApp.Dto
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }
        [RequiredIf(nameof(Role), "Employee")]
        public string? EmployersEmail { get; set; }
        [RequiredIf(nameof(Role),"Employer")]
        public string? CompanyName { get; set; }
    }
}
