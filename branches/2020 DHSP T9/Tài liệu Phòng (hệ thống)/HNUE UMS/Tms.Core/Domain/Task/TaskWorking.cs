using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Conversion;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Task
{
    [Table("Task_Working")]
    public class TaskWorking : BaseEntity
    {
        public int YearId { get; set; }
        public int StaffId { get; set; }
        public int CategoryId { get; set; }
        public int ConversionId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Amount { get; set; }
        public bool Approved { get; set; }
        public bool IgnoreDuplicated { get; set; }
        public int ApproveBy { get; set; }
        public double Time { get; set; }
        public DateTime ApproveOn { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public virtual ConversionWorking Conversion { get; set; }
        public virtual Staff Staff { get; set; }
        public bool IsLocked { get; set; }
    }
}