using System;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Task;
using Ums.Services.Base;
using Ums.Services.Calculate;
using Ums.Services.Personnel;
using Ums.Services.Security;

namespace Ums.Services.Task
{
    public class TaskReservedService : Service<TaskReserved>, ITaskReservedService
    {
        private readonly IStaffService _staffService;
        private readonly IStaffCalculate _staffCalculate;
        public TaskReservedService(DbContext context, IContextService contextService, IStaffService staffService, IStaffCalculate staffCalculate) : base(context, contextService)
        {
            _staffService = staffService;
            _staffCalculate = staffCalculate;
        }

        public void CreateReserved(int fromYearId, int toYearId)
        {
            var stave = _staffService.Gets(moved: 2, retired: 2).ToList();
            foreach (var staff in stave)
            {
                var duty = _staffCalculate.CalculateDutyTime(fromYearId, staff);
                var teachingDone = _staffCalculate.CalculateTeachingTime(fromYearId, staff.Id, approved: 1);
                var researchingDone = _staffCalculate.CalculateResearchingTime(fromYearId, staff.Id, 1);
                var workingDone = _staffCalculate.CalculateWorkingTime(fromYearId, staff.Id, 1);
                var previous = GetReserved(staff.Id, fromYearId);
                var m = new TaskReserved
                {
                    StaffId = staff.Id,
                    FromYearId = fromYearId,
                    ToYearId = toYearId,
                    TeachingDuty = duty.Teaching,
                    ResearchingDuty = duty.Researching,
                    WorkingDuty = duty.Working,
                    TeachingDone = teachingDone,
                    ResearchingDone = researchingDone,
                    WorkingDone = workingDone,
                    PreviousReserved = previous,
                    Updated = DateTime.Now
                };
                m.CalculateResearchingReserved();
                if (m.Reserved > 0)
                {
                    Insert(m);
                }
            }
        }

        public double GetReserved(int staffId, int toYearId)
        {
            return Gets().Where(i => i.StaffId == staffId && i.ToYearId == toYearId).FirstOrDefault()?.Reserved ?? 0;
        }

        public IQueryable<TaskReserved> Gets(int yearId = 0, int departmentId = 0, int staffId = 0)
        {
            var lst = base.Gets();
            if (yearId > 0)
            {
                lst = lst.Where(i => i.ToYearId == yearId);
            }
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.Staff.DepartmentId == departmentId || i.Staff.TeachingInId == departmentId);
            }
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            return lst;
        }

        public void DeleteReserved(int toYearId)
        {
            foreach (var researchingReserved in base.Gets().Where(i => i.ToYearId == toYearId).ToList())
            {
                base.HardDelete(researchingReserved);
            }
        }
    }
}