using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using VacationManagementApp.Enums;

namespace VacationManagementApp.Models
{
    public class Vacation
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public int HowManyDays { set; get; }
        [Required]
        public int EmployeeId { set; get; }
        public VacationState state { set; get; } = VacationState.Waiting;
        
        public string? WhereTo { set; get; }


    }
}
