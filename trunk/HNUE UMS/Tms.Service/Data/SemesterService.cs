using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class SemesterService : Service<Semester>, ISemesterService
    {
        public SemesterService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public IQueryable<Semester> GetByYear(int yearId)
        {
            return Gets().Where(i => i.YearId == yearId);
        }
    }
}