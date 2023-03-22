using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.System;
using Ums.Services.Base;

namespace Ums.Services.System
{
    public interface IStaffUserService : IService<StaffUser>
    {
        StaffUser GetByUsername(string username);
        StaffUser GetByEmail(string email);
        StaffUser GetByStaff(int staffId);
        void Generate(IEnumerable<Staff> stave);
        void ResetPassword(Staff staff);
        void ChangePassword(int userId, string newPassword);
        StaffUser Get(string account);
        void ResetAll(IEnumerable<Staff> stave);
        int InitRecoverPassword(string email, string url);
        bool RecoverPassword(string email, string token);
        IQueryable<StaffUser> Gets(int departmentId = 0, string keyword = "");
    }
}