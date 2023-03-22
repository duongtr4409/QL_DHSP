using System;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;
using Ums.Services.Data;
using Ums.Services.Organize;
using Ums.Services.Security;

namespace Ums.Services.Personnel
{
    public class StaffPositionService : Service<StaffPosition>, IStaffPositionService
    {
        private readonly IYearService _yearService;
        private readonly IDepartmentService _departmentService;
        public StaffPositionService(DbContext context, IContextService contextService, IYearService yearService, IDepartmentService departmentService) : base(context, contextService)
        {
            _yearService = yearService;
            _departmentService = departmentService;
        }

        public IQueryable<StaffPosition> GetsByStaff(int staffId)
        {
            return base.Gets().Where(i => i.StaffId == staffId);
        }

        public IQueryable<StaffPosition> GetFacultyPositions(int yearId, int departmentId, int staffId = 0, int positionId = 0)
        {
            var year = _yearService.Get(yearId);
            var lst = base.Gets().Where(i => i.Position.IsPublic && i.Start <= year.To && i.End >= year.From && i.Staff.DepartmentId == departmentId);
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            if (positionId > 0)
            {
                lst = lst.Where(i => i.PositionId == positionId);
            }
            return lst;
        }

        public IQueryable<StaffPosition> GetSchoolPositions(int departmentId = 0, int staffId = 0)
        {
            var lst = base.Gets();
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.Staff.DepartmentId == departmentId);
            }
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            return lst;
        }

        public bool CheckPosition(int id, int departmentId, int positionId, DateTime start, DateTime end)
        {
            var max = _departmentService.Get(departmentId)?.MaxPositions.FirstOrDefault(i => i.Key == positionId)?.Value ?? 0;
            if (max == 0) return true;
            var assigned = base.Gets().Count(i => i.Id != id && i.Staff.DepartmentId == departmentId && i.PositionId == positionId && i.Start <= end && i.End >= start);
            return assigned < max;
        }

        public override void Delete(StaffPosition entity)
        {
            HardDelete(entity);
        }
    }
}