using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;

namespace Ums.Services.Conversion
{
    public interface IWorkingService : IService<ConversionWorking>
    {
        IEnumerable<ConversionWorking> GetTree();
        IQueryable<ConversionWorking> Gets(int categoryId);
    }
}