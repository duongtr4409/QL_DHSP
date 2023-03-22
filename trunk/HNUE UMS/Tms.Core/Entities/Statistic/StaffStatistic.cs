using System;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;
using Ums.Core.Types;

namespace Ums.Core.Entities.Statistic
{
    public class StaffStatistic
    {
        private double _teachDuty;
        private double _researchDuty;
        private double _workDuty;
        private double _teachReal;
        private double _teachDone;
        private double _teachPaid;
        private double _researchDone;
        private double _researchReserved;
        private double _workDone;
        private double _money;
        private double _deducted;
        private double _departmentRatio;

        public StaffStatistic()
        {
            DepartmentRatio = 1;
            OverTimeRatio = 1;
        }
        public Year Year { get; set; }
        public Classify Classify { get; set; }
        public Department TeachingIn { get; set; }
        public Department Department { get; set; }
        public double DepartmentRatio
        {
            get => Math.Round(_departmentRatio, 2);
            set => _departmentRatio = value;
        }
        public Grade Grade { get; set; }
        public Staff Staff { get; set; }
        public double DegreeRatio => Staff.Degree.Ratio;
        public double TitleRatio => Staff.Title.GetRatio(Staff.SalaryLevel);
        public double DutyRatio { private get; set; } = 1;
        public double TeachDuty
        {
            get => Math.Round(_teachDuty * DutyRatio, 2);
            set => _teachDuty = value;
        }
        public double TeachReal
        {
            get => Math.Round(_teachReal, 2);
            set => _teachReal = value;
        }
        public double TeachDone
        {
            get => Math.Round(_teachDone, 2);
            set => _teachDone = value;
        }
        public double TeachPaid
        {
            get => Math.Round(_teachPaid, 2);
            set => _teachPaid = value;
        }
        public double TeachComplement => TeachDone < TeachDuty ? Math.Round(TeachDuty - TeachDone, 2) : 0;
        public double TeachOver
        {
            get
            {
                var over = TeachDone - TeachDuty - ResearchComplement - WorkComplement;
                return over > 0 ? Math.Round(over, 2) : 0;
            }
        }
        public double OnlyTeachOver
        {
            get
            {
                var over = TeachDone + TeachPaid - TeachDuty;
                return over > 0 ? Math.Round(over, 2) : 0;
            }
        }
        public double ResearchDuty
        {
            get => Math.Round(_researchDuty * DutyRatio, 2);
            set
            {
                if (Staff.Title.TitleTypeId != (int)TitleTypes.Lecturer)
                {
                    _researchDuty = 0;
                }
                else
                {
                    _researchDuty = value;
                }
            }
        }
        public double ResearchReserved
        {
            get => Math.Round(_researchReserved, 2);
            set
            {
                if (Staff.Title.TitleTypeId != (int)TitleTypes.Lecturer)
                {
                    _researchReserved = 0;
                }
                else
                {
                    _researchReserved = value;
                }
            }
        }
        public double ResearchDone
        {
            get => Math.Round(_researchDone, 2);
            set
            {
                if (Staff.Title.TitleTypeId != (int)TitleTypes.Lecturer && Staff.Title.TitleTypeId != (int)TitleTypes.PracticeTrainer)
                {
                    _researchDone = 0;
                }
                else
                {
                    _researchDone = value;
                }
            }
        }
        public double ResearchComplement => ResearchDone + ResearchReserved < ResearchDuty ? Math.Round(ResearchDuty - ResearchDone - ResearchReserved, 2) : 0;
        public double OnlyResearchOver
        {
            get
            {
                var over = ResearchDone + ResearchReserved - ResearchDuty;
                return over > 0 ? Math.Round(over, 2) : 0;
            }
        }
        public double WorkDuty
        {
            get => Math.Round(_workDuty * DutyRatio, 2);
            set => _workDuty = value;
        }
        public double WorkDone
        {
            get => Math.Round(_workDone, 2);
            set => _workDone = value;
        }
        public double WorkDoneTotal => Math.Round(_workDone + WorkFromResearch, 2);
        public double WorkComplement => WorkDoneTotal < WorkDuty ? Math.Round(WorkDuty - WorkDoneTotal, 2) : 0;
        public double MaxWorkFromResearch { get; set; }
        public double WorkFromResearch
        {
            get
            {
                if (Staff.Title.TitleTypeId != (int)TitleTypes.PracticeTrainer)
                {
                    return 0;
                }
                if (ResearchDone > MaxWorkFromResearch)
                {
                    return Math.Round(MaxWorkFromResearch, 2);
                }
                return Math.Round(ResearchDone, 2);
            }
        }
        public double OnlyWorkOver
        {
            get
            {
                var over = WorkDoneTotal - WorkDuty;
                return over > 0 ? Math.Round(over, 2) : 0;
            }
        }
        public double TotalDuty => Math.Round(TeachDuty + ResearchDuty + WorkDuty, 2);
        public double TotalDone => Math.Round(TeachDone + TeachPaid + ResearchDone + ResearchReserved + WorkDoneTotal, 2);
        public double OverTimeRatio { get; set; }
        public double OverTime
        {
            get
            {
                var r = TeachDone - ResearchComplement - WorkComplement - TeachDuty;
                return r > 0 ? Math.Round(r, 2) : 0;
            }
        }
        public double MoneyTime => Math.Round(OverTime * DepartmentRatio * OverTimeRatio, 2);
        public bool Done { get; set; }
        public double Money
        {
            get => Math.Round(_money, 2);
            set => _money = value;
        }
        public double MoneyDeducted
        {
            get => Math.Round(_deducted, 2);
            set => _deducted = value;
        }
        public double MoneyRemain => Math.Round(Money - MoneyDeducted, 2);

        public bool TeachingFinished => TeachDone + TeachPaid >= TeachDuty;
        public bool ResearchingFinished => ResearchDone + ResearchReserved >= ResearchDuty;
        public bool WorkFinished => WorkDone + WorkFromResearch >= WorkDuty;
    }
}