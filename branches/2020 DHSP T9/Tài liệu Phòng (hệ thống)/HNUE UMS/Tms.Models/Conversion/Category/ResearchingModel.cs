using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Conversion.Category
{
    public class ResearchingModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}