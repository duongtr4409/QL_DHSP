using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;

namespace Ums.Services.Conversion
{
    public interface IResearchingService : IService<ConversionResearching>
    {
        IQueryable<ConversionResearching> Gets(int categoryId = 0);
        IQueryable<ConversionResearching> Gets(string code);
        string[] GetCodes();
        string[] GetGroupCodes();
    }
}