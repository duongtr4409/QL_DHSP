using System.Data.Entity;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion.Type
{
    public class TeachingTypeService : Service<ConversionTeachingType>, ITeachingTypeService
    {
        public TeachingTypeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}
