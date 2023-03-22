using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Hnue.Helper;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.System;
using Ums.Core.Entities.Mailing;
using Ums.Services.Base;
using Ums.Services.Mailing;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class StaffUserService : Service<StaffUser>, IStaffUserService
    {
        private readonly IEmailSender _emailSender;
        private readonly ISettingService _settingService;
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

        public int InitRecoverPassword(string email, string url)
        {
            var u = GetByEmail(email);
            if (u == null) return 0;
            u.RecoverPasswordCode = Common.RandomString(128);
            Update(u);
            var subject = _settingService.GetValue("EMAIL_RECOVER_PASSWORD_SUBJECT");
            var domain = _settingService.GetValue("DOMAIN");
            var body = _settingService.GetValue("EMAIL_RECOVER_PASSWORD_BODY");
            body = body.Replace("{{link}}", $"<a href='{domain}{url}?email={email}&token={u.RecoverPasswordCode}'>Bấm vào đây để khởi tạo lại mật khẩu</a>");
            _emailSender.SendAsync(subject, body, new List<EmailAddress> { new EmailAddress(email) });
            return 1;
        }

        public bool RecoverPassword(string email, string token)
        {
            var u = GetByEmail(email);
            if (u != null && u.RecoverPasswordCode == token)
            {
                ResetPassword(u);
                return true;
            }
            return false;
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

        public void ResetPassword(Staff staff)
        {
            var u = GetByStaff(staff.Id);
            var pass = staff.Code.Substring(staff.Code.LastIndexOf('.') + 1);
            u.Username = staff.Code.Replace(" ", "").Replace(".", "");
            u.Password = Common.Md5(pass);
            u.Hint = pass;
            Update(u);
        }

        public void ResetPassword(StaffUser u)
        {
            var pass = u.Staff.Code.Substring(u.Staff.Code.LastIndexOf('.') + 1);
            u.Username = u.Staff.Code.Replace(" ", "").Replace(".", "");
            u.Password = Common.Md5(pass);
            u.Hint = pass;
            Update(u);
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var u = Get(userId);
            u.Password = Common.Md5(newPassword);
            u.Hint = "";
            Update(u);
        }

        public StaffUserService(DbContext context, IContextService contextService, ISettingService settingService, IEmailSender emailSender) : base(context, contextService)
        {
            _settingService = settingService;
            _emailSender = emailSender;
        }
    }
}