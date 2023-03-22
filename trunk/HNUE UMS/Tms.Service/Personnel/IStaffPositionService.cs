using System;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Services.Base;

namespace Ums.Services.Personnel
{
    public interface IStaffPositionService : IService<StaffPosition>
    {
        IQueryable<StaffPosition> GetsByStaff(int staffId);
        IQueryable<StaffPosition> GetFacultyPositions(int yearId, int departmentId, int staffId = 0, int positionId = 0);
        IQueryable<StaffPosition> GetSchoolPositions(int departmentId = 0, int staffId = 0);
        bool CheckPosition(int id, int departmentId, int positionId, DateTime start, DateTime end);
    }
}