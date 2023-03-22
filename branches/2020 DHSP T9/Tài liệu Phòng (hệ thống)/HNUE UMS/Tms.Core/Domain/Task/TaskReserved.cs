using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Task
{
    [Table("Task_Reserved")]
    public class TaskReserved : BaseEntity
    {
        private double _teachingDuty;
        private double _teachingDone;
        private double _researchingDuty;
        private double _researchingDone;
        private double _workingDuty;
        private double _workingDone;
        private double _previousReserved;
        private double _reserved;
        public int StaffId { get; set; }
        public int ToYearId { get; set; }
        public int FromYearId { get; set; }
        public double TeachingDuty
        {
            get => Math.Round(_teachingDuty, 2);
            set => _teachingDuty = value;
        }
        public double TeachingDone
        {
            get => Math.Round(_teachingDone, 2);
            set => _teachingDone = value;
        }
        public double ResearchingDuty
        {
            get => Math.Round(_researchingDuty, 2);
            set => _researchingDuty = value;
        }
        public double ResearchingDone
        {
            get => Math.Round(_researchingDone, 2);
            set => _researchingDone = value;
        }
        public double WorkingDuty
        {
            get => Math.Round(_workingDuty, 2);
            set => _workingDuty = value;
        }
        public double WorkingDone
        {
            get => Math.Round(_workingDone, 2);
            set => _workingDone = value;
        }
        public double PreviousReserved
        {
            get => Math.Round(_previousReserved, 2);
            set => _previousReserved = value;
        }
        public double Reserved
        {
            get => Math.Round(_reserved, 2);
            set => _reserved = value;
        }
        public DateTime Updated { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Year FromYear { get; set; }
    }
}
