using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Task;
using Ums.Services.Base;

namespace Ums.Services.Task
{
    public interface IResearchingService : IService<TaskResearching>
    {
        IQueryable<TaskResearching> Gets(int yearId = 0, int departmentId = 0, int staffId = 0, int approved = 0, string code = "", string group = "", int locked = 0, int phased = 0, int approvedOrPhrased = 0);
        void Approve(int id, bool value, int approveStaffId);
        bool SetIgnoreDuplicate(int id, bool ignore);
        void SetApproveMessage(int id, string message);
        bool SavePhase(int id, double time);
        bool CompletePhase(int id, bool value);
        void LockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0, int phased = 0);
        void UnlockAll(int yearId, int departmentId = 0, int staffId = 0, int approved = 0, int phased = 0);
        List<TaskResearching> FindDuplicates(int yearId, int departmentId);
    }
}