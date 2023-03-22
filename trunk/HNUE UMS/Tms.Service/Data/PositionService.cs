using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Data;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Data
{
    public class PositionService : Service<Position>, IPositionService
    {
        private readonly IPositionGroupService _groupService;
        public PositionService(DbContext context, IContextService contextService, IPositionGroupService groupService) : base(context, contextService)
        {
            _groupService = groupService;
        }

        public IQueryable<Position> Gets(int groupId)
        {
            var catIds = _groupService.GetChildrenIds(groupId);
            return base.Gets().Where(i => catIds.Contains(i.GroupId));
        }

        public IQueryable<Position> GetFacultyPositions(int groupId = 0)
        {
            return Gets(groupId).Where(i => i.IsPublic);
        }
    }
}