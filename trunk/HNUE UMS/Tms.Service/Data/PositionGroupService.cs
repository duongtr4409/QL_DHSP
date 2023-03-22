using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class PositionGroupService : Service<PositionGroup>, IPositionGroupService
    {

        public int[] GetChildrenIds(int parentId)
        {
            var c = new List<int> { parentId };
            foreach (var cat in Gets().Where(i => i.ParentId == parentId).ToList())
            {
                c.AddRange(GetChildrenIds(cat.Id));
            }
            return c.ToArray();
        }

        public List<PositionGroup> GetTree(PositionGroup parent = null, string sap = "...", string add = "")
        {
            var r = new List<PositionGroup>();
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

        public PositionGroupService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}