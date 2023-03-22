using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion
{
    public class TeachingService : Service<ConversionTeaching>, ITeachingService
    {
        public TeachingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public override IQueryable<ConversionTeaching> Gets()
        {
            return base.Gets().OrderBy(i => i.Name);
        }

        public IQueryable<ConversionTeaching> Gets(int categoryId = 0)
        {
            var lst = base.Gets();
            if (categoryId > 0)
            {
                lst = lst.Where(i => i.CategoryId == categoryId);
            }
            return lst;
        }
    }
}