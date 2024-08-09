using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BusinessLogic.Enums;

namespace BusinessLogic.Models
{
    public class Vacation
    {
        [Key]
        public int Id { set; get; }
        [Required]
        [DisplayName("How many days")]
        public int HowManyDays { set; get; }
        public string EmployeeId { set; get; }
        public VacationState state { set; get; } = VacationState.Waiting;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime When { set; get; }
        [JsonIgnore]
        public Employee Employee { set; get; }


    }
}
