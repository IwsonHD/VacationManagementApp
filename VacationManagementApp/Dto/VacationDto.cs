using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacationManagementApp.Enums;
using VacationManagementApp.Models;

namespace VacationManagementApp.Dto
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
