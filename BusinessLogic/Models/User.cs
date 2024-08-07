using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class User :IdentityUser
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //[Required]
        //public string EmailAddress { get; set; }

        [Required]
        [StringLength(9)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
