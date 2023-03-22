using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Connect;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Connect
{
    public class NoticeService : Service<Notice>, INoticeService
    {
        public NoticeService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public IQueryable<Notice> GetNotices()
        {
            return Gets().Where(i => i.Public);
        }
    }
}