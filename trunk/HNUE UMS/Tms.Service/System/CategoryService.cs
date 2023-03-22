using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class CategoryService : Service<SystemCategory>, ICategoryService
    {
        private readonly IFunctionService _functionService;
        public CategoryService(DbContext context, IContextService contextService, IFunctionService functionService) : base(context, contextService)
        {
            _functionService = functionService;
        }

        public override IQueryable<SystemCategory> Gets()
        {
            return base.Gets().OrderBy(i => i.Order);
        }

        public override void Delete(SystemCategory entity)
        {
            var functions = _functionService.Gets(categoryId: entity.Id).ToList();
            foreach (var f in functions)
            {
                _functionService.Delete(f);
            }
            base.Delete(entity);
        }
    }
}