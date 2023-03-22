using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Domain.Data;

namespace Ums.Models.Data
{
    [NotMapped]
    public class YearModel : Year
    {
        [Required]
        public new string From { get; set; }
        [Required]
        public new string To { get; set; }
    }
}