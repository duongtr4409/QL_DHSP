using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Task;
using Ums.Services.Base;

namespace Ums.Services.Task
{
    public interface ITeachingService : IService<TaskTeaching>
    {
        IQueryable<TaskTeaching> Gets(int yearId = 0,
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
            string keyword = "");

        List<TaskTeaching> FindDuplicates(int yearId, int departmentId = 0, int gradeId = 0, int ignored = 0);

        bool Assign(int id, int staffId);

        bool Approve(int id, bool approve, int approveUserId);

        bool Confirm(int id, bool confirm);

        void LockAll(int yearId, int departmentId, int gradeId, int staffId, int status);

        void UnlockAll(int yearId, int departmentId, int gradeId, int staffId, int status);

        bool SetIgnoreDuplicate(int id, bool ignore);
    }
}