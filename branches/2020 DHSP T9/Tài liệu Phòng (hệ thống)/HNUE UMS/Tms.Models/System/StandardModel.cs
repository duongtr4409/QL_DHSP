using System.ComponentModel.DataAnnotations;

namespace Ums.Models.System
{
    public class StandardModel
    {
        public int Id { get; set; }
        public int Researching { get; set; }
        public int Teaching { get; set; }
        public int Working { get; set; }
        [Range(1, int.MaxValue)]
        public int TitleId { get; set; }
    }
}
