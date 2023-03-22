using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Ums.Core.Domain.Data;

namespace Ums.Models.Data
{
    [NotMapped]
    public class PositionModel : Position
    {
        public int CategoryId { get; set; }
        [Required]
        public new string Name { get; set; }
        public new double Quota { get; set; } = 1;
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}