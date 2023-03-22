using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Hnue.Helper;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion.Category
{
    public class WorkingService : Service<ConversionWorkingCategory>, Ums.Services.Conversion.Category.IWorkingService
    {
        public List<ConversionWorkingCategory> GetTree(ConversionWorkingCategory parent = null, string sap = "...", string add = "")
        {
            var r = new List<ConversionWorkingCategory>();
            var lst = parent == null ? Gets().Where(i => i.ParentId == 0).ToList() : Gets().Where(i => i.ParentId == parent.Id).ToList();
            lst = lst.Select(i => i.Clone()).ToList();
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

        public WorkingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}