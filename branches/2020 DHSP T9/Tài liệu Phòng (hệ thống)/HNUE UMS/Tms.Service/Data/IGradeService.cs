using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.System;
using Ums.Services.Base;

namespace Ums.Services.Data
{
    public interface IGradeService : IService<Grade>
    {
        IQueryable<Grade> GetsbyUser(StaffUser user);
    }
}