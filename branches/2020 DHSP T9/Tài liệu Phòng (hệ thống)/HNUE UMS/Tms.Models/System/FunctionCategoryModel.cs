using System.ComponentModel.DataAnnotations;

namespace Ums.Models.System
{
    public class FunctionCategoryModel
    {
        public int Id { get; set; }
        public int Order { get; set; }
        //[Required]
        public string Icon { get; set; }
        [Required]
        public string Name { get; set; }
    }
}