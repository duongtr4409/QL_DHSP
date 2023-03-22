using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class FunctionService : Service<Function>, IFunctionService
    {
        private readonly IRoleFunctionService _roleFunction;
        public override void Delete(Function entity)
        {
            var rfs = _roleFunction.Gets().Where(i => i.FunctionId == entity.Id).ToList();
            foreach (var rf in rfs)
            {
                _roleFunction.Delete(rf);
            }
            base.Delete(entity);
        }

        public IQueryable<Function> Gets(int categoryId = 0)
        {
            var lst = base.Gets();
            if (categoryId > 0)
            {
                lst = lst.Where(i => i.CategoryId == categoryId);
            }
            return lst;
        }

        public FunctionService(DbContext context, IContextService contextService, IRoleFunctionService roleFunction) : base(context, contextService)
        {
            _roleFunction = roleFunction;
        }
    }
}