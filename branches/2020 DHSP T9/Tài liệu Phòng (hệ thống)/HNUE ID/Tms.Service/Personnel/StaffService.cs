using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;
using Ums.Services.Security;
using Ums.Services.System;

namespace Ums.Services.Personnel
{
    public class StaffService : Service<Staff>, IStaffService
    {
        public StaffService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }


        public IQueryable<Staff> Gets(int departmentId = 0, int moved = 0, int retired = 0, string keyword = "", int titleId = 0, int probation = 0, int movedOrRetired = 0, int teachingInId = 0, int departmentType = 0)
        {
            var lst = base.Gets();
            if (departmentType > 0)
            {
                lst = lst.Where(i => i.Department.TypeId == departmentType || i.TeachingIn.TypeId == departmentType);
            }
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.DepartmentId == departmentId || i.TeachingInId == departmentId);
            }
            if (teachingInId > 0)
            {
                lst = lst.Where(i => i.TeachingInId == teachingInId);
            }
            if (titleId > 0)
            {
                lst = lst.Where(i => i.TitleId == titleId);
            }
            if (probation > 0)
            {
                lst = lst.Where(i => i.IsProbation == (probation == 1));
            }
            if (moved > 0)
            {
                lst = lst.Where(i => i.IsMoved == (moved == 1));
            }
            if (movedOrRetired > 0)
            {
                lst = lst.Where(i => i.IsMoved == (movedOrRetired == 1) || i.IsRetired == (movedOrRetired == 1));
            }
            if (retired > 0)
            {
                lst = lst.Where(i => i.IsRetired == (retired == 1));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                lst = lst.Where(i => i.Name.Contains(keyword) || i.Code.Contains(keyword));
            }
            return lst.OrderBy(i => i.Name);
        }
    }
}