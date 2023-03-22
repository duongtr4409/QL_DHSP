using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Task;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Task
{
    public class ResearchingService : Service<TaskResearching>, IResearchingService
    {
        public ResearchingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public IQueryable<TaskResearching> Gets(int yearId = 0, int departmentId = 0, int staffId = 0, int approved = 0, string code = "", string group = "", int locked = 0, int phased = 0, int approvedOrPhrased = 0)
        {
            var lst = base.Gets();
            if (yearId > 0)
            {
                lst = lst.Where(i => i.YearId == yearId);
            }
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.Staff.DepartmentId == departmentId || i.Staff.TeachingInId == departmentId);
            }
            if (approved > 0)
            {
                lst = lst.Where(i => i.Approved == (approved == 1));
            }
            if (approvedOrPhrased > 0)
            {
                lst = lst.Where(i => i.Approved == (approvedOrPhrased == 1) || i.PhaseCompleted == (approvedOrPhrased == 1));
            }
            if (phased > 0)
            {
                lst = lst.Where(i => i.PhaseCompleted == (phased == 1));
            }
            if (locked > 0)
            {
                lst = lst.Where(i => i.IsLocked == (locked == 1));
            }
            if (!string.IsNullOrEmpty(code))
            {
                lst = lst.Where(i => i.Conversion.Code == code);
            }
            if (!string.IsNullOrEmpty(group))
            {
                lst = lst.Where(i => i.Conversion.Code.Contains(group));
            }
            return lst;
        }

        public List<TaskResearching> FindDuplicates(int yearId, int departmentId)
        {
            var s = Gets(yearId, departmentId).ToArray();
            var ds = new List<TaskResearching>();
            var temp = new List<TaskResearching>();
            for (var i = 0; i < s.Length; i++)
            {
                temp.Clear();
                for (var j = i + 1; j < s.Length; j++)
                {
                    if (s[i].ConversionId != s[j].ConversionId) continue;
                    if (s[i].StaffId != s[j].StaffId) continue;
                    if (s[i].Staff.DepartmentId != s[j].Staff.DepartmentId) continue;
                    if (s[i].Quantity != s[j].Quantity) continue;
                    if (s[i].Name.Replace(" ", "") != s[j].Name.Replace(" ", "")) continue;
                    if (s[i].Desc?.Replace(" ", "") != s[j].Desc?.Replace(" ", "")) continue;
                    temp.Add(s[j]);
                }
                if (temp.Count > 0)
                {
                    ds.Add(s[i]);
                    ds.AddRange(temp);
                }
            }
            return ds;
        }

        public void Approve(int id, bool value, int approveStaffId)
        {
            var r = Get(id);
            r.Approved = value;
            r.ApproveBy = approveStaffId;
            r.ApproveOn = DateTime.Now;
            if (value)
            {
                r.ApproveMessage = "";
            }
            Update(r);
        }

        public bool SetIgnoreDuplicate(int id, bool ignore)
        {
            var o = Get(id);
            o.IgnoreDuplicated = ignore;
            Update(o);
            return ignore;
        }

        public void SetApproveMessage(int id, string message)
        {
            var r = Get(id);
            r.ApproveMessage = message;
            Update(r);
        }

        public bool SavePhase(int id, double time)
        {
            var r = Get(id);
            r.WorkTime = time;
            Update(r);
            return true;
        }

        public bool CompletePhase(int id, bool value)
        {
            var c = Get(id);
            c.PhaseCompleted = value;
            Update(c);
            return value;
        }

        public void LockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0, int phased = 0)
        {
            var lst = Gets(yearId, departmentId, staffId, approved, phased: phased).ToList();
            foreach (var r in lst)
            {
                r.IsLocked = true;
                base.Update(r);
            }
        }

        public void UnlockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0, int phased = 0)
        {
            var lst = Gets(yearId, departmentId, staffId, approved, phased: phased).ToList();
            foreach (var r in lst)
            {
                r.IsLocked = false;
                base.Update(r);
            }
        }

        public override void Update(TaskResearching entity)
        {
            if (GetColumns(entity.Id, i => i.IsLocked)) return;
            base.Update(entity);
        }
    }
}