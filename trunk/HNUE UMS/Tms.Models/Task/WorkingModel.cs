using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ums.Models.Task
{
    public class WorkingModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int CategoryId { get; set; }
        public int ConversionId { get; set; }
        [Required]
        public double Amount { get; set; } = 1;
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}