using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion.Category
{
    public class TeachingService : Service<ConversionTeachingCategory>, ITeachingService
    {
        public TeachingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public IQueryable<ConversionTeachingCategory> Gets(int parentId)
        {
            return base.Gets().Where(i => i.ParentId == parentId);
        }

        public List<ConversionTeachingCategory> GetTree(ConversionTeachingCategory parent = null, string sap = "...", string add = "")
        {
            var r = new List<ConversionTeachingCategory>();
            var lst = parent == null ? Gets().Where(i => i.ParentId == 0).ToList() : Gets().Where(i => i.ParentId == parent.Id).ToList();
            foreach (var c in lst)
            {
                c.Name = add + c.Name;
            }
            if (parent != null) r.Add(parent);
            foreach (var c in lst.OrderBy(i => i.Name))
            {
                r.AddRange(GetTree(c, sap, sap + add));
            }
            return r;
        }

    }
}