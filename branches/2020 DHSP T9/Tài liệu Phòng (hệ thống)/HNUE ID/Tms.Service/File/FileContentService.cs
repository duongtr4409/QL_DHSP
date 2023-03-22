using System.Data.Entity;
using Ums.Core.Domain.File;
using Ums.Services.Base;
using Ums.Services.Connect;
using Ums.Services.Security;

namespace Ums.Services.File
{
    public class FileContentService : Service<FileContent>, IFileContentService
    {
        private IContextService _contextService;
        public FileContentService(DbContext context, IContextService contextService) : base(context, contextService)
        {
            _contextService = contextService;
        }
    }
}