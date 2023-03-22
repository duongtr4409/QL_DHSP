using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Data
{
    public class SemesterModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        public string Desc { get; set; }
        public int YearId { get; set; }
    }
}