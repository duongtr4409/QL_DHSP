using System.Data.Entity;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class DegreeService : Service<Degree>, IDegreeService
    {
        public DegreeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}