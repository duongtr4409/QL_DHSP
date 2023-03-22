using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Personnel
{
    public class StaffPositionModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public int CategoryId { get; set; }
        public int PositionId { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
    }
}
