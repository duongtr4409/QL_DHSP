using System.Data.Entity;
using Ums.Core.Domain.Conversion;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Conversion
{
    public class StandardService : Service<ConversionStandard>, IStandardService
    {
        public StandardService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}