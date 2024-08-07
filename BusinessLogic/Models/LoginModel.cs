using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class LoginModel
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
