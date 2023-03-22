using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;

namespace Ums.Services.Conversion.Category
{
    public interface ITeachingService : IService<ConversionTeachingCategory>
    {
        IQueryable<ConversionTeachingCategory> Gets(int parentId);
        List<ConversionTeachingCategory> GetTree(ConversionTeachingCategory parent = null, string sap = "...", string add = "");
    }
}