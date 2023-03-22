using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.Users;
using Ums.Services.Base;

namespace Ums.Services.Users
{
    public interface IStaffUserService : IService<StaffUser>
    {
        StaffUser GetByUsername(string username);
        StaffUser GetByEmail(string email);
        StaffUser GetByStaff(int staffId);
        void Generate(IEnumerable<Staff> stave);
        StaffUser Get(string account);
        void ResetAll(IEnumerable<Staff> stave);
        IQueryable<StaffUser> Gets(int departmentId = 0, string keyword = "");
    }
}