using System.Linq;
using Ums.Core.Domain.Connect;
using Ums.Services.Base;

namespace Ums.Services.Connect
{
    public interface IFeedbackService : IService<Feedback>
    {
        IQueryable<Feedback> GetFeedbacks(int staffId);
        void Response(int id, string response);
    }
}