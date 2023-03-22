using System.Linq;
using Ums.Core.Domain.Connect;
using Ums.Services.Base;

namespace Ums.Services.Connect
{
    public interface INoticeService : IService<Notice>
    {
        IQueryable<Notice> GetNotices();
    }
}