using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hnue.Helper;
using Ums.Core.Base;
using Ums.Core.Domain.Conversion;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Task
{
    [Table("Task_Researching")]
    public class TaskResearching : BaseEntity
    {
        public int YearId { get; set; }
        public int StaffId { get; set; }
        public int ConversionId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Attach { get; set; }
        public int Quantity { get; set; }
        public int Members { get; set; }
        public int StartYear { get; set; }
        public bool Approved { get; set; }
        public bool IgnoreDuplicated { get; set; }
        public bool PhaseCompleted { get; set; }
        public int ApproveBy { get; set; }
        public double WorkTime { get; set; } = 1;
        public double Time { get; set; }
        public DateTime ApproveOn { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string ApproveMessage { get; set; }
        public bool IsLocked { get; set; }
        public int SyncId { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual ConversionResearching Conversion { get; set; }
        [NotMapped]
        public string[] Attaches
        {
            get => Attach.GetSplit('↔');
            set => Attach = value.JoinArray('↔');
        }
        public int SyncStaffId { get; set; }
    }
}