using System.Linq;
using Ums.Core.Domain.Task;
using Ums.Services.Base;

namespace Ums.Services.Task
{
    public interface ITaskReservedService : IService<TaskReserved>
    {
        void CreateReserved(int fromYearId, int toYearId);
        double GetReserved(int staffId, int toYearId);
        IQueryable<TaskReserved> Gets(int yearId = 0, int departmentId = 0, int staffId = 0);
        void DeleteReserved(int toYearId);
    }
}