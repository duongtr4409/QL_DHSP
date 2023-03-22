using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Task;
using Ums.Services.Base;
using Ums.Services.Personnel;
using Ums.Services.Security;

namespace Ums.Services.Task
{
    public class WorkingService : Service<TaskWorking>, IWorkingService
    {
        private readonly IStaffService _staffService;
        public WorkingService(DbContext context, IContextService contextService, IStaffService staffService) : base(context, contextService)
        {
            _staffService = staffService;
        }

        public IQueryable<TaskWorking> Gets(int yearId = 0, int departmentId = 0, int staffId = 0, int approved = 0, string keyword = "", int locked = 0)
        {
            var lst = base.Gets();
            if (yearId > 0)
            {
                lst = lst.Where(i => i.YearId == yearId);
            }
            if (departmentId > 0)
            {
                var staffIds = _staffService.GetTrainerIn(departmentId).Select(i => i.Id).ToArray();
                lst = lst.Where(i => staffIds.Contains(i.StaffId));
            }
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            if (approved > 0)
            {
                lst = lst.Where(i => i.Approved == (approved == 1));
            }
            if (locked > 0)
            {
                lst = lst.Where(i => i.IsLocked == (locked == 1));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                lst = lst.Where(i => i.Name.Contains(keyword));
            }
            return lst;
        }

        public List<TaskWorking> FindDuplicates(int yearId, int departmentId)
        {
            var s = Gets(yearId, departmentId).ToArray();
            var ds = new List<TaskWorking>();
            var temp = new List<TaskWorking>();
            for (var i = 0; i < s.Length; i++)
            {
                temp.Clear();
                for (var j = i + 1; j < s.Length; j++)
                {
                    if (s[i].ConversionId != s[j].ConversionId) continue;
                    if (s[i].StaffId != s[j].StaffId) continue;
                    if (s[i].Staff.DepartmentId != s[j].Staff.DepartmentId) continue;
                    if (Math.Abs(s[i].Amount - s[j].Amount) > 0) continue;
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

        public void Approve(int id, bool approve, int approveStaffId)
        {
            var ws = Get(id);
            ws.Approved = approve;
            ws.ApproveBy = approveStaffId;
            ws.ApproveOn = DateTime.Now;
            Update(ws);
        }

        public void ApproveAll(int yeId, int deId, bool approve, int approveStaffId)
        {
            foreach (var w in base.Gets().Where(i => i.YearId == yeId & i.Staff.DepartmentId == deId).ToList())
            {
                w.Approved = approve;
                w.ApproveBy = approveStaffId;
                w.ApproveOn = DateTime.Now;
                Update(w);
            }
        }

        public bool SetIgnoreDuplicate(int id, bool ignore)
        {
            var o = Get(id);
            o.IgnoreDuplicated = ignore;
            Update(o);
            return ignore;
        }

        public override void Update(TaskWorking entity)
        {
            if (GetColumns(entity.Id, i => i.IsLocked)) return;
            base.Update(entity);
        }

        public void LockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0)
        {
            var lst = Gets(yearId, departmentId, staffId, approved, locked: 2).ToList();
            foreach (var r in lst)
            {
                r.IsLocked = true;
                base.Update(r);
            }
        }

        public void UnlockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0)
        {
            var lst = Gets(yearId, departmentId, staffId, approved, locked: 1).ToList();
            foreach (var r in lst)
            {
                r.IsLocked = false;
                base.Update(r);
            }
        }
    }
}