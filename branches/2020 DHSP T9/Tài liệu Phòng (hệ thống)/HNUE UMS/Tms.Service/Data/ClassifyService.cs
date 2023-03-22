using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class ClassifyService : Service<Classify>, IClassifyService
    {
        public ClassifyService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public List<KeyValuePair<string, int>> ClassifiesStatistic(int departmentId = 0, int yearId = 0)
        {
            var lst = Gets().OrderBy(i => i.Name).ToList();
            if (departmentId > 0)
            {
                return lst.Select(i => new KeyValuePair<string, int>(i.Name, i.StaffClassifies.Count(j => j.YearId == yearId && j.Staff.DepartmentId == departmentId))).ToList();
            }
            return lst.Select(i => new KeyValuePair<string, int>(i.Name, i.StaffClassifies.Count(j => j.YearId == yearId))).ToList();
        }
    }
}