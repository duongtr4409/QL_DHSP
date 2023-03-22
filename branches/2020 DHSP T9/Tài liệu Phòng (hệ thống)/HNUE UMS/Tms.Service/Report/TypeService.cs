using System.Data.Entity;
using Ums.Core.Domain.Report;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Report
{
    public class TypeService : Service<ReportType>, ITypeService
    {
        public TypeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}