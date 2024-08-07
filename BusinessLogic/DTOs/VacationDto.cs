using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BusinessLogic.Enums;
using BusinessLogic.Models;

namespace BusinessLogic.DTOs
{
    public class VacationDto
    {
 
        [Required]
        [DisplayName("How many days")]
        public int HowManyDays { set; get; }
        //public string EmployeeId { set; get; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime When { set; get; }

    }
}
