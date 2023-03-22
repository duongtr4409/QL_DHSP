using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;

namespace Ums.Services.Data
{
    public interface IYearService : IService<Year>
    {
        IQueryable<Year> GetAll();
    }
}