using System;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Connect;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Connect
{
    public class MessageService : Service<Message>, IMessageService
    {
        private IContextService _contextService;
        public MessageService(DbContext context, IContextService contextService) : base(context, contextService)
        {
            _contextService = contextService;
        }

        public IQueryable<Message> Gets(int fromId = 0, int toId = 0, int seen = 0, DateTime last = default(DateTime), int[] groupIds = null)
        {
            var lst = base.Gets();
            if (groupIds != null)
            {
                lst = lst.Where(i => groupIds.Contains(i.FromId) && groupIds.Contains(i.ToId));
            }
            if (fromId > 0)
            {
                lst = lst.Where(i => i.FromId == fromId);
            }
            if (toId > 0)
            {
                lst = lst.Where(i => i.ToId == toId);
            }
            if (seen > 0)
            {
                lst = lst.Where(i => i.Seen == (seen == 1));
            }
            if (last != default(DateTime))
            {
                lst = lst.Where(i => DbFunctions.TruncateTime(i.Created) >= DbFunctions.TruncateTime(last));
            }
            return lst;
        }

        public void SendMessage(int toId, string message)
        {
            Insert(new Message
            {
                Content = message,
                FromId = _contextService.GetCurrentUser().Id,
                ToId = toId
            });
        }
    }
}