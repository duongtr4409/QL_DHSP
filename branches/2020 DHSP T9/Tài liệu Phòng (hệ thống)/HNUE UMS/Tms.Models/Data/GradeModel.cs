using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Domain.Data;

namespace Ums.Models.Data
{
    [NotMapped]
    public class GradeModel : Grade
    {
        [Required]
        public new string Name { get; set; }
    }
}