using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Core.Types;
using Ums.Services.Base;
using Ums.Services.Security;
using Ums.Services.System;

namespace Ums.Services.Personnel
{
    public class StaffService : Service<Staff>, IStaffService
    {
        private readonly IStaffPositionService _staffPositionService;
        private readonly IStaffUserService _userService;
        public StaffService(DbContext context, IContextService contextService, IStaffPositionService staffPositionService, IStaffUserService userService) : base(context, contextService)
        {
            _staffPositionService = staffPositionService;
            _userService = userService;
        }

        public IQueryable<Staff> GetTrainerIn(int departmentId = 0, int moved = 0, int retired = 0, int titleId = 0, string keyword = "", int movedOrRetired = 0, int teachingInId = 0)
        {
            var types = new[] { (int)TitleTypes.Lecturer, (int)TitleTypes.PracticeTrainer };
            var lst = Gets(departmentId, moved, retired, keyword, titleId, movedOrRetired: movedOrRetired, teachingInId: teachingInId, departmentType: 1).Where(i => types.Contains(i.Title.TitleTypeId));
            return lst.OrderBy(i => i.Name);
        }

        public IQueryable<Staff> GetLecturerIn(int departmentId = 0, int moved = 0, int retired = 0)
        {
            var types = new[] { (int)TitleTypes.Lecturer };
            var lst = Gets(departmentId, moved, retired, departmentType: 1).Where(i => types.Contains(i.Title.TitleTypeId));
            return lst.OrderBy(i => i.Name);
        }

        public IQueryable<Staff> GetTeacherIn(int departmentId = 0, int moved = 0, int retired = 0)
        {
            var types = new[] { (int)TitleTypes.PracticeTrainer };
            var lst = Gets(departmentId, moved, retired, departmentType: 1).Where(i => types.Contains(i.Title.TitleTypeId));
            return lst.OrderBy(i => i.Name);
        }

        public IQueryable<Staff> Gets(int departmentId = 0, int moved = 0, int retired = 0, string keyword = "", int titleId = 0, int probation = 0, int movedOrRetired = 0, int teachingInId = 0, int departmentType = 0, int degreeId = 0)
        {
            var lst = base.Gets();
            if (departmentType > 0)
            {
                lst = lst.Where(i => i.Department.TypeId == departmentType || i.TeachingIn.TypeId == departmentType);
            }
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.DepartmentId == departmentId || i.TeachingInId == departmentId);
            }
            if (teachingInId > 0)
            {
                lst = lst.Where(i => i.TeachingInId == teachingInId);
            }
            if (titleId > 0)
            {
                lst = lst.Where(i => i.TitleId == titleId);
            }
            if (degreeId > 0)
            {
                lst = lst.Where(i => i.DegreeId == degreeId);
            }
            if (probation > 0)
            {
                lst = lst.Where(i => i.IsProbation == (probation == 1));
            }
            if (moved > 0)
            {
                lst = lst.Where(i => i.IsMoved == (moved == 1));
            }
            if (movedOrRetired > 0)
            {
                lst = lst.Where(i => i.IsMoved == (movedOrRetired == 1) || i.IsRetired == (movedOrRetired == 1));
            }
            if (retired > 0)
            {
                lst = lst.Where(i => i.IsRetired == (retired == 1));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                lst = lst.Where(i => i.Name.Contains(keyword) || i.Code.Contains(keyword));
            }
            return lst.OrderBy(i => i.Name);
        }

        public override void Delete(Staff entity)
        {
            foreach (var sp in _staffPositionService.GetsByStaff(entity.Id).ToList())
            {
                _staffPositionService.Delete(sp);
            }
            var u = _userService.GetByStaff(entity.Id);
            if (u != null) _userService.Delete(u);
            base.Delete(entity);
        }
    }
}