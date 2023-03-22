using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Hnue.Helper;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.Users;
using Ums.Core.Entities.Mailing;
using Ums.Services.Base;
using Ums.Services.Mailing;
using Ums.Services.Security;

namespace Ums.Services.Users
{
    public class StaffUserService : Service<StaffUser>, IStaffUserService
    {
        public StaffUserService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public StaffUser GetByUsername(string username)
        {
            var u = Gets().FirstOrDefault(i => i.Username.ToLower() == username.ToLower());
            return u ?? GetByEmail(username);
        }

        public StaffUser GetByEmail(string email)
        {
            return Gets().FirstOrDefault(i => i.Email.ToLower() == email.ToLower());
        }

        public StaffUser Get(string account)
        {
            var u = GetByUsername(account);
            return u ?? GetByEmail(account);
        }

        public StaffUser GetByStaff(int staffId)
        {
            return Gets().FirstOrDefault(i => i.StaffId == staffId);
        }

        public void Generate(IEnumerable<Staff> stave)
        {
            foreach (var s in stave.ToList())
            {
                if (GetByStaff(s.Id) != null) continue;
                var pass = s.Code.Substring(s.Code.LastIndexOf('.') + 1);
                Insert(new StaffUser
                {
                    Username = s.Code.Replace(" ", "").Replace(".", ""),
                    Password = Common.Md5(pass),
                    StaffId = s.Id,
                    Hint = pass,
                    PasswordChanged = false
                });
            }
        }

        public void ResetAll(IEnumerable<Staff> stave)
        {
            foreach (var s in stave.ToList())
            {
                var u = GetByStaff(s.Id);
                var pass = s.Code.Substring(s.Code.LastIndexOf('.') + 1);
                if (u != null)
                {
                    u.Password = Common.Md5(pass);
                    u.Hint = pass;
                    Update(u);
                }
                else
                {
                    Insert(new StaffUser
                    {
                        Username = s.Code.Replace(" ", "").Replace(".", ""),
                        Password = Common.Md5(pass),
                        StaffId = s.Id,
                        Hint = pass
                    });
                }
            }
        }

        public IQueryable<StaffUser> Gets(int departmentId = 0, string keyword = "")
        {
            var lst = base.Gets();
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.Staff.DepartmentId == departmentId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                lst = lst.Where(i => i.Staff.Name.Contains(keyword) || i.Staff.Code.Contains(keyword) || i.Email.Contains(keyword));
            }
            return lst;
        }
    }
}