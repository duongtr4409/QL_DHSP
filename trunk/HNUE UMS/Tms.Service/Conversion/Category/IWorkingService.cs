using System.Collections.Generic;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;

namespace Ums.Services.Conversion.Category
{
    public interface IWorkingService:IService<ConversionWorkingCategory>
    {
        List<ConversionWorkingCategory> GetTree(ConversionWorkingCategory parent = null, string sap = "...", string add = "");
    }
}