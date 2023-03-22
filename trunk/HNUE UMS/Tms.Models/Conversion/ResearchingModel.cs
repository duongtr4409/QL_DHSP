using System.Collections.Generic;
using System.Web.Mvc;

namespace Ums.Models.Conversion
{
    public class ResearchingModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double MaxMaterialTime { get; set; }
        public double Ratio { get; set; }
        public int MemberOffset { get; set; }
        public double Factor { get; set; }
        public string Unit { get; set; }
        public bool IsDirectly { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public bool HasMember { get; set; }
        public int EquivalentQuantity { get; set; }
    }
}
