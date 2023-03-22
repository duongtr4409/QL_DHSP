using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Domain.File;

namespace Ums.Models.File
{
    [NotMapped]
    public class FileContentModel : FileContent
    {

    }

    public class ContentShareModel
    {
        public int FileId { get; set; }
        public int[] UserIds { get; set; } 
    }

}
