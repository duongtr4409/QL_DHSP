using System.Data.Entity;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class TitleTypeService : Service<TitleType>, ITitleTypeService
    {
        public TitleTypeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}