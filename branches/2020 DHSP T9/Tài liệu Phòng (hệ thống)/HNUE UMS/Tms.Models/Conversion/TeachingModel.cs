using System.Collections.Generic;
using System.Web.Mvc;

namespace Ums.Models.Conversion
{
    public class TeachingModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int TeachingTypeId { get; set; }
        public int GroupSize { get; set; }
        public string Name { get; set; }
        public double Ratio { get; set; }
        public string Unit { get; set; }
        public string Desc { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> TeachingTypes { get; set; }
    }
}
