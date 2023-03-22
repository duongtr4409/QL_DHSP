using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion
{
    public class WorkingService : Service<ConversionWorking>, IWorkingService
    {
        private readonly Ums.Services.Conversion.Category.IWorkingService _categoryService;

        public override IQueryable<ConversionWorking> Gets()
        {
            return base.Gets().OrderBy(i => i.Name);
        }

        public IEnumerable<ConversionWorking> GetTree()
        {
            var lst = new List<ConversionWorking>();
            var cats = _categoryService.Gets();
            foreach (var c in cats.ToList())
            {
                lst.Add(new ConversionWorking { Name = c.Name.ToUpper(), Id = 0 });
                foreach (var cw in Gets(c.Id).ToList())
                {
                    cw.Name = "....." + cw.Name;
                    lst.Add(cw);
                }
            }
            return lst;
        }

        public IQueryable<ConversionWorking> Gets(int categoryId)
        {
            return Gets().Where(i => i.CategoryId == categoryId);
        }

        public WorkingService(DbContext context, IContextService contextService, Ums.Services.Conversion.Category.IWorkingService categoryService) : base(context, contextService)
        {
            _categoryService = categoryService;
        }
    }
}