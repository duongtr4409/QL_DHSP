using System.Data.Entity;
using System.Linq;
using Hnue.Helper;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion
{
    public class ResearchingService : Service<ConversionResearching>, IResearchingService
    {
        public ResearchingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public IQueryable<ConversionResearching> Gets(int categoryId = 0)
        {
            var lst = base.Gets();
            if (categoryId > 0)
            {
                lst = lst.Where(i => i.CategoryId == categoryId);
            }
            return lst;
        }
        public IQueryable<ConversionResearching> Gets(string code)
        {
            return Gets().Where(i => i.Code == code);
        }
        public string[] GetCodes()
        {
            return Gets().Where(i => !string.IsNullOrEmpty(i.Code)).Select(i => i.Code).Distinct().OrderBy(i => i).ToArray();
        }
        public string[] GetGroupCodes()
        {
            return Gets().Where(i => !string.IsNullOrEmpty(i.Code))
                .Select(i => i.Code).ToArray()
                .Select(i => i.GetSplit('-')[0])
                .Distinct().OrderBy(i => i).ToArray();
        }
    }
}