using System.Collections.Generic;
using System.Web.Mvc;

namespace Ums.Models.Conversion
{
    public class WorkingModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Ratio { get; set; }
        public int MemberOffset { get; set; }
        public double Factor { get; set; }
        public string Unit { get; set; }
        public int Amount { get; set; }
        public string Desc { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}