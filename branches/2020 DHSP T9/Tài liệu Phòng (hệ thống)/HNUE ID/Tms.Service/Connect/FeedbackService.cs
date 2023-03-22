using System;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Connect;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Connect
{
    public class FeedbackService : Service<Feedback>, IFeedbackService
    {
        private readonly IContextService _contextService;
        public FeedbackService(DbContext context, IContextService contextService, IContextService contextService1) : base(context, contextService)
        {
            _contextService = contextService1;
        }

        public IQueryable<Feedback> GetFeedbacks(int staffId)
        {
            return Gets().Where(i => i.StaffId == staffId);
        }

        public void Response(int id, string response)
        {
            var f = Get(id);
            f.Response = response;
            //f.ResponserId = _contextService.GetCurrentUser().StaffId;
            f.Responsed = DateTime.Now;
            Update(f);
        }
    }
}