using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;

namespace Ums.Services.Data
{
    public interface ISemesterService : IService<Semester>
    {
        IQueryable<Semester> GetByYear(int yearId);
    }
}