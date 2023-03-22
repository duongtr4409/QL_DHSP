using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.System;

namespace Ums.Core.Domain.Report
{
    [Table("Report_Data")]
    public class ReportData : BaseEntity
    {
        public int YearId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int MoneyReportId { get; set; }
        public int ReportId { get; set; }
        public int SemesterId { get; set; }
        public int GradeId { get; set; }
        public int DepartmentId { get; set; }
        public int StaffFilter { get; set; }
        public string Desc { get; set; }
        public string Data { get; set; }
        public string File { get; set; }
        public bool ApplyFacultyRatio { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool Run { get; set; }
        public int CreateBy { get; set; }
        public bool Locked { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual ReportType Type { get; set; }
        [ForeignKey(nameof(CreateBy))]
        public virtual StaffUser Creator { get; set; }
        public virtual Year Year { get; set; }
    }
}