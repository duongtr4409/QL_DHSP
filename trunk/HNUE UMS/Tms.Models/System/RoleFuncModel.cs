using System.Collections.Generic;
using System.Web.Mvc;

namespace Ums.Models.System
{
    public class RoleFuncModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<int> Selecteds { get; set; } = new List<int>();
        public IList<FuncGroup> List { get; set; } = new List<FuncGroup>();
    }

    public class FuncGroup
    {
        public string GroupName { get; set; }
        public IList<SelectListItem> List { get; set; } = new List<SelectListItem>();
    }
}