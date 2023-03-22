using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;

namespace Ums.Services.Personnel
{
    public interface IStaffClassifyService : IService<StaffClassify>
    {
        void UpdateStaffClassify(int yearId, int staffId, int classId);
        StaffClassify GetClassify(int yearId, int staffId);
        void Lock(int yearId, bool locked, int staffId = 0);
        IQueryable<StaffClassify> Gets(int yearId, int locked = 0);
    }
}