using System.Collections.Generic;
using System.Web.Mvc;

namespace Ums.Models.Common
{
    public class ListModel
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public IList<int> Selecteds { get; set; }
        public IList<SelectListItem> List { get; set; }
        public ListModel()
        {
            Selecteds = new List<int>();
            List = new List<SelectListItem>();
        }
    }
}