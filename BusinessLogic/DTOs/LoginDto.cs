using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessLogic.DTOs
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
