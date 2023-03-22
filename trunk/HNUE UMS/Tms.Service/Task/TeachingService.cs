using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Hnue.Helper;
using Ums.Core.Common;
using Ums.Core.Domain.Task;
using Ums.Services.Base;
using Ums.Services.Security;
using Ums.Services.System;

namespace Ums.Services.Task
{
    public class TeachingService : Service<TaskTeaching>, ITeachingService
    {
        private readonly ISettingService _settingService;
        public TeachingService(DbContext context, IContextService contextService, ISettingService settingService) : base(context, contextService)
        {
            _settingService = settingService;
        }

        public IQueryable<TaskTeaching> Gets(int yearId = 0,
            int departmentId = 0,
            int forDepartmentId = 0,
            int gradeId = 0,
            int conversionId = 0,
            int staffId = 0,
            int paid = 0,
            int confirmed = 0,
            int approved = 0,
            int duplicated = 0,
            int invited = 0,
            int semesterId = 0,
            int assigned = 0,
            int locked = 0,
            string course = "",
            string cls = "",
            string linkedPartner = "",
            string specializing = "",
            string keyword = "")
        {
            var lst = base.Gets();
            if (locked > 0)
            {
                lst = lst.Where(i => i.IsLocked == (locked == 1));
            }
            if (assigned > 0)
            {
                lst = lst.Where(i => assigned == 1 && i.StaffId > 0 || assigned > 1 && i.StaffId == 0);
            }
            if (invited > 0)
            {
                lst = lst.Where(i => i.Invited == (invited == 1));
            }
            if (semesterId > 0)
            {
                lst = lst.Where(i => i.SemesterId == semesterId);
            }
            if (confirmed > 0)
            {
                lst = lst.Where(i => i.Confirmed == (confirmed == 1));
            }
            if (approved > 0)
            {
                lst = lst.Where(i => i.Approved == (approved == 1));
            }
            if (paid > 0)
            {
                lst = lst.Where(i => i.Paid == (paid == 1));
            }
            if (yearId > 0)
            {
                lst = lst.Where(i => i.YearId == yearId);
            }
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.DepartmentId == departmentId);
            }
            if (forDepartmentId > 0)
            {
                lst = lst.Where(i => i.ForDepartmentId == forDepartmentId);
            }
            if (gradeId > 0)
            {
                lst = lst.Where(i => i.GradeId == gradeId);
            }
            if (conversionId > 0)
            {
                lst = lst.Where(i => i.ConversionId == conversionId);
            }
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            if (duplicated > 0)
            {
                lst = lst.Where(i => i.IgnoreDuplicated == (duplicated == 1));
            }
            if (!string.IsNullOrEmpty(cls) && cls != "0")
            {
                lst = lst.Where(i => i.Class.Contains(cls));
            }
            if (!string.IsNullOrEmpty(linkedPartner) && linkedPartner != "0")
            {
                lst = lst.Where(i => i.LinkedPartner.Contains(linkedPartner));
            }
            if (!string.IsNullOrEmpty(specializing) && specializing != "0")
            {
                lst = lst.Where(i => i.Specializing.Contains(specializing));
            }
            if (!string.IsNullOrEmpty(course) && course != "0")
            {
                lst = lst.Where(i => i.Course.Contains(course));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                lst = lst.Where(i => i.Name.Contains(keyword));
            }
            return lst;
        }

        public List<TaskTeaching> FindDuplicates(int yearId, int departmentId = 0, int gradeId = 0, int ignored = 0)
        {
            var lst = Gets(yearId, departmentId, gradeId: gradeId);
            if (ignored > 0)
            {
                lst = lst.Where(i => i.IgnoreDuplicated == (ignored == 1));
            }
            var s = lst.ToArray();
            var ds = new List<TaskTeaching>();
            var temp = new List<TaskTeaching>();
            for (var i = 0; i < s.Length; i++)
            {
                temp.Clear();
                for (var j = i + 1; j < s.Length; j++)
                {
                    if (s[i].GradeId != s[j].GradeId) continue;
                    if (s[i].ConversionId != s[j].ConversionId) continue;
                    if (s[i].StaffId != s[j].StaffId) continue;
                    if (s[i].DepartmentId != s[j].DepartmentId) continue;
                    if (s[i].Size != s[j].Size) continue;
                    if (Math.Abs(s[i].LessonTime - s[j].LessonTime) > 0) continue;
                    if (s[i].Name.Replace(" ", "") != s[j].Name.Replace(" ", "")) continue;
                    if (s[i].TeachingTime != null && s[j].TeachingTime != null && !s[i].TeachingTime.Contains(s[j].TeachingTime) && !s[j].TeachingTime.Contains(s[i].TeachingTime)) continue;
                    if (s[i].Class != null && s[i].Class.Trim() != "" && s[j].Class != null && s[j].Class.Trim() != "" && !s[i].Class.Contains(s[j].Class) && !s[j].Class.Contains(s[i].Class)) continue;
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

        public bool Assign(int id, int staffId)
        {
            if (_settingService.GetValue(Settings.TEACHING_LOCK).ToBool())
            {
                return false;
            }
            var s = Get(id);
            s.StaffId = staffId;
            s.Approved = false;
            Update(s);
            return true;
        }

        public bool Approve(int id, bool approve, int approveUserId)
        {
            if (_settingService.GetValue(Settings.TEACHING_LOCK).ToBool())
            {
                return false;
            }
            var s = Get(id);
            if (s == null || s.Approved && !s.Confirmed) return false;
            s.Approved = approve;
            s.ApproveBy = approveUserId;
            s.ApproveOn = DateTime.Now;
            Update(s);
            return true;
        }

        public bool Confirm(int id, bool confirm)
        {
            if (_settingService.GetValue(Settings.TEACHING_LOCK).ToBool())
            {
                return false;
            }
            var s = Get(id);
            if (s == null) return false;
            s.Confirmed = confirm;
            Update(s);
            return true;
        }

        public void LockAll(int yearId, int departmentId, int gradeId, int staffId, int status)
        {
            var lst = Gets(yearId, departmentId, gradeId: gradeId, approved: status, staffId: staffId, locked: 2).ToList();
            foreach (var teaching in lst)
            {
                teaching.IsLocked = true;
                Update(teaching);
            }
        }

        public void UnlockAll(int yearId, int departmentId, int gradeId, int staffId, int status)
        {
            var lst = Gets(yearId, departmentId, gradeId: gradeId, approved: status, staffId: staffId, locked: 1).ToList();
            foreach (var teaching in lst)
            {
                teaching.IsLocked = false;
                base.Update(teaching);
            }
        }

        public bool SetIgnoreDuplicate(int id, bool ignore)
        {
            if (_settingService.GetValue(Settings.TEACHING_LOCK).ToBool())
            {
                return false;
            }
            var o = Get(id);
            o.IgnoreDuplicated = ignore;
            base.Update(o);
            return ignore;
        }

        public override void Update(TaskTeaching entity)
        {
            if (_settingService.GetValue(Settings.TEACHING_LOCK).ToBool())
            {
                return;
            }
            if (GetColumns(entity.Id, i => i.IsLocked)) return;
            var o = GetColumns(entity.Id, i => new { Createby = i.CreateBy, i.Created, i.IgnoreDuplicated, i.SyncId, i.SyncOn, i.SyncStaffId });
            entity.Updated = DateTime.Now;
            entity.CreateBy = o.Createby;
            entity.Created = o.Created;
            entity.IgnoreDuplicated = o.IgnoreDuplicated;
            entity.SyncId = o.SyncId;
            entity.SyncOn = o.SyncOn;
            entity.SyncStaffId = o.SyncStaffId;
            base.Update(entity);
        }
    }
}