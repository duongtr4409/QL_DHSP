using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Task
{
    public class TeachingModel
    {
        [Required]
        public int Id { get; set; }
        public int YearId { get; set; }
        [Range(1, int.MaxValue)]
        public int GradeId { get; set; }
        public int ConversionTeachingId { get; set; }
        public int DepartmentId { get; set; }
        public int ForDepartmentId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public int StaffId { get; set; }
        [Required]
        public double LessonTime { get; set; } = 30;
        public string TeachingTime { get; set; }
        [Required]
        public string Name { get; set; }
        public string Course { get; set; }
        public string Class { get; set; }
        [Required]
        public double Size { get; set; } = 1;
        [Required]
        public bool Paid { get; set; }
        public string Desc { get; set; }
        public bool Invited { get; set; }
        public string Lecturer { get; set; }
        public int InvitedDegreeId { get; set; }
        public string InvitedPartner { get; set; }
        public string Special { get; set; } 
        public string LinkedPartner { get; set; }
        public int SemesterId { get; set; }
    }
}