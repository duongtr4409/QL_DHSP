using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Domain.System;

namespace Ums.Models.System
{
    [NotMapped]
    public class FunctionModel : Function
    {
        [Required]
        public new string Icon { get; set; }
        [Required]
        public new string Key { get; set; }
        [Required]
        public new string Name { get; set; }
        [Required]
        public new string Url { get; set; }
    }
}