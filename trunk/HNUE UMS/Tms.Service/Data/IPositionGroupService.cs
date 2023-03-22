using System.Collections.Generic;
using Ums.Core.Domain.Data;
using Ums.Services.Base;

namespace Ums.Services.Data
{
    public interface IPositionGroupService : IService<PositionGroup>
    {
        int[] GetChildrenIds(int parentId);
        List<PositionGroup> GetTree(PositionGroup parent = null, string sap = "...", string add = "");
    }
}