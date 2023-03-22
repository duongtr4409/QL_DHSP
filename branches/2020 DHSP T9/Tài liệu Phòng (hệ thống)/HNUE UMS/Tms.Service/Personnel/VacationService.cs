using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Personnel
{
    public class VacationService : Service<Vacation>, IVacationService
    {
        public Vacation GetByStaff(int staffId)
        {
            return Gets().FirstOrDefault(i => i.StaffId == staffId);
        }

        public IQueryable<Vacation> GetsbyDepartment(int departmentId)
        {
            return Gets().Where(i => i.Staff.DepartmentId == departmentId || i.Staff.TeachingInId == departmentId);
        }

        public override void Delete(object id)
        {
            HardDelete(Get(id));
        }

        public VacationService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}