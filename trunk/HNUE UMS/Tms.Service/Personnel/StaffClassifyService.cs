using System;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.Personnel
{
    public class StaffClassifyService : Service<StaffClassify>, IStaffClassifyService
    {
        private readonly IContextService _contextService;
        public StaffClassifyService(DbContext context, IContextService contextService) : base(context, contextService)
        {
            _contextService = contextService;
        }

        public void UpdateStaffClassify(int yearId, int staffId, int classId)
        {
            var s = base.Gets().FirstOrDefault(i => i.StaffId == staffId && i.YearId == yearId) ?? new StaffClassify();
            s.ClassifyId = classId;
            s.StaffId = staffId;
            s.YearId = yearId;
            s.IndexerId = _contextService.GetCurrentUser().StaffId;
            s.IndexedOn = DateTime.Now;
            InsertOrUpdate(s);
        }

        public StaffClassify GetClassify(int yearId, int staffId)
        {
            return base.Gets().FirstOrDefault(i => i.YearId == yearId && i.StaffId == staffId);
        }

        public void Lock(int yearId, bool locked, int staffId = 0)
        {
            var lst = Gets().Where(i => i.YearId == yearId && i.IsLocked == !locked);
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            foreach (var c in lst.ToList())
            {
                c.IsLocked = locked;
                base.Update(c);
            }
        }

        public IQueryable<StaffClassify> Gets(int yearId, int locked = 0)
        {
            var lst = Gets().Where(i => i.YearId == yearId);
            if (locked > 0)
            {
                lst = lst.Where(i => i.IsLocked == (locked == 1));
            }
            return lst;
        }

        public override void Update(StaffClassify entity)
        {
            if (Get(entity.Id).IsLocked) return;
            base.Update(entity);
        }
    }
}