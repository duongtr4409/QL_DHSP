using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Data
{
    public class PositionGroupModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}