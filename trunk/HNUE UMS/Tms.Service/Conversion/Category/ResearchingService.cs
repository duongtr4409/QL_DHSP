using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Hnue.Helper;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion.Category
{
    public class ResearchingService : Service<ConversionResearchingCategory>, IResearchingService
    {
        public ResearchingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public IQueryable<ConversionResearchingCategory> Gets(int parentId)
        {
            var lst = base.Gets();
            if (parentId > -1)
            {
                lst = lst.Where(i => i.ParentId == parentId);
            }
            return lst;
        }

        public List<ConversionResearchingCategory> GetTree(ConversionResearchingCategory parent = null, string sap = "...", string add = "")
        {
            var r = new List<ConversionResearchingCategory>();
            var lst = parent == null ? Gets().Where(i => i.ParentId == 0).OrderBy(i => i.Id).ToList() : Gets().Where(i => i.ParentId == parent.Id).OrderBy(i => i.Id).ToList();
            lst = lst.Select(c => c.Clone()).ToList();
            foreach (var c in lst)
            {
                c.Name = add + c.Name;
            }
            if (parent != null) r.Add(parent);
            foreach (var c in lst)
            {
                r.AddRange(GetTree(c, sap, sap + add));
            }
            return r;
        }
    }
}