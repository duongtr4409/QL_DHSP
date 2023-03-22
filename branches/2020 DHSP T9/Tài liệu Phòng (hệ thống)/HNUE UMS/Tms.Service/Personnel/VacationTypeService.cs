using System.Data.Entity;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Personnel
{
    public class VacationTypeService : Service<VacationType>, IVacationTypeService
    {
        public VacationTypeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }
    }
}