using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class YearService : Service<Year>, IYearService
    {
        public YearService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public new IQueryable<Year> Gets()
        {
            return base.Gets().Where(i => i.Visible).OrderByDescending(i => i.From);
        }

        public IQueryable<Year> GetAll()
        {
            return base.Gets();
        }
    }
}