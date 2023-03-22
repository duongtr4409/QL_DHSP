using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Data
{
    public class DegreeModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double Ratio { get; set; }
        public int Order { get; set; }
    }
}