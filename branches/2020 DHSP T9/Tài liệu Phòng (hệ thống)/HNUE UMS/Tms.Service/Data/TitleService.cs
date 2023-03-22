using System.Data.Entity;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class TitleService : Service<Title>, ITitleService
    {
        public TitleService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}