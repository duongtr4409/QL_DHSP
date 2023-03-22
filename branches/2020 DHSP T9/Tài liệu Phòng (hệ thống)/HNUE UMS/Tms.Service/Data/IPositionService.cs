using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;

namespace Ums.Services.Data
{
    public interface IPositionService : IService<Position>
    {
        IQueryable<Position> Gets(int groupId);
        IQueryable<Position> GetFacultyPositions(int groupId = 0);
    }
}