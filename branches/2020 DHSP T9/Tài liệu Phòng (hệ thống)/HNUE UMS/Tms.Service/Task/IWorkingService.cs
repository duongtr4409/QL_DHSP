using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Task;
using Ums.Services.Base;

namespace Ums.Services.Task
{
    public interface IWorkingService : IService<TaskWorking>
    {
        IQueryable<TaskWorking> Gets(int yearId = 0, int departmentId = 0, int staffId = 0, int approved = 0, string keyword = "", int locked = 0);
        void Approve(int id, bool approve, int approveStaffId);
        void ApproveAll(int yeId, int deId, bool approve, int approveStaffId);
        bool SetIgnoreDuplicate(int id, bool ignore);
        List<TaskWorking> FindDuplicates(int yearId, int departmentId);
        void LockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0);
        void UnlockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0);
    }
}