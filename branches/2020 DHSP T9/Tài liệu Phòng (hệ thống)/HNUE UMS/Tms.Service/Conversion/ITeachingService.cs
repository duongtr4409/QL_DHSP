using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;

namespace Ums.Services.Conversion
{
    public interface ITeachingService : IService<ConversionTeaching>
    {
        IQueryable<ConversionTeaching> Gets(int categoryId = 0);
    }
}