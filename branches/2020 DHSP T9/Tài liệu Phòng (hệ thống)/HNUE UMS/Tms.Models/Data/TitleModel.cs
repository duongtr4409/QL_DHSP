using System.Collections.Generic;
using System.Web.Mvc;

namespace Ums.Models.Data
{
    public class TitleModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Level1 { get; set; }
        public double Level2 { get; set; }
        public double Level3 { get; set; }
        public double Level4 { get; set; }
        public double Level5 { get; set; }
        public double Level6 { get; set; }
        public double Level7 { get; set; }
        public double Level8 { get; set; }
        public double Level9 { get; set; }
        public double Level10 { get; set; }
        public double Level11 { get; set; }
        public double Level12 { get; set; }
        public int TitleTypeId { get; set; }
        public IEnumerable<SelectListItem> TitleTypes { get; set; }
        public string ShortName { get; set; }
    }
}