using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Conversion;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.System;

namespace Ums.Core.Domain.Task
{
    [Table("Task_Teaching")]
    public class TaskTeaching : BaseEntity
    {
        public int YearId { get; set; }
        public int GradeId { get; set; }
        public int DepartmentId { get; set; }
        public int ForDepartmentId { get; set; }
        public int StaffId { get; set; }
        public int ConversionId { get; set; }
        public double LessonTime { get; set; } 
        public string Name { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string TeachingTime { get; set; }
        public string Desc { get; set; }
        public string Course { get; set; }
        public string Class { get; set; }
        public double Size { get; set; }
        public bool Confirmed { get; set; }
        public bool Approved { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveOn { get; set; } = DateTime.Now;
        public bool Paid { get; set; }
        public bool Invited { get; set; }
        public string Lecturer { get; set; }
        public int InvitedDegreeId { get; set; }
        public string InvitedPartner { get; set; }
        public string Specializing { get; set; }
        public string LinkedPartner { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public int UpdateBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsLocked { get; set; }
        public bool IgnoreDuplicated { get; set; }
        public int CreateBy { get; set; }
        [ForeignKey(nameof(CreateBy))]
        public virtual StaffUser Creator { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual Department Department { get; set; }
        public virtual Department ForDepartment { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual ConversionTeaching Conversion { get; set; }
        [ForeignKey(nameof(UpdateBy))]
        public virtual StaffUser Updater { get; set; }
        public int SemesterId { get; set; }
        public string SyncId { get; set; }
        public int SyncStaffId { get; set; }
        public DateTime SyncOn { get; set; } = DateTime.Now;
    }
}