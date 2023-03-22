using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Personnel
{
    public class VacationModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public int TypeId { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
    }
}