using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Users;

namespace Ums.Core.Domain.File
{
    [Table("File_Access")]
    public class FileAccess : BaseEntity
    {
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string KeyId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public virtual User Creator { get; set; }
        public virtual FileContent File { get; set; }
    }
}