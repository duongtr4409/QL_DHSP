using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;

namespace Ums.Services.Conversion.Category
{
    public interface IResearchingService : IService<ConversionResearchingCategory>
    {
        IQueryable<ConversionResearchingCategory> Gets(int parentId);
        List<ConversionResearchingCategory> GetTree(ConversionResearchingCategory parent = null, string sap = "...", string add = "");
    }
}