using System;
using System.Linq;
using Ums.Core.Domain.Connect;
using Ums.Services.Base;

namespace Ums.Services.Connect
{
    public interface IMessageService : IService<Message>
    {
        IQueryable<Message> Gets(int fromId = 0, int toId = 0, int seen = 0, DateTime last = default(DateTime), int[] groupIds = null);
        void SendMessage(int toId, string message);
    }
}