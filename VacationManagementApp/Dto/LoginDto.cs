using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VacationManagementApp.Dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
