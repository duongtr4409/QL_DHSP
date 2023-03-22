using Hnue.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.Users;
using Ums.Core.Entities.Mailing;
using Ums.Core.Entities.Shared;
using Ums.Models.Security;
using Ums.Services.Base;
using Ums.Services.Mailing;
using Ums.Services.Security;
using Ums.Services.Users;

namespace Ums.Services.System
{
    public class UserService : Service<User>, IUserService
    {
        private readonly ISettingService _settingService;
        private readonly IEmailSender _emailSender;
        public UserService(DbContext context, IContextService contextService, ISettingService settingService, IEmailSender emailSender) : base(context, contextService)
        {
            _settingService = settingService;
            _emailSender = emailSender;
        }

        public User CreateUser(RegisterModel model)
        {
            var user = !string.IsNullOrEmpty(model.UserKey) ? GetByUserKey(model.UserKey) : null;
            if (user == null)
            {
                user = new User();
            }
            model.CopyTo(user);
            user.Password = Common.Md5(model.Password);
            user.UserType = model.UserType;
            user.UserKey = string.IsNullOrEmpty(model.UserKey) ? Guid.NewGuid().ToString() : model.UserKey;
            user.Username = model.Email;
            user.UserType= "GUEST";
            var id = InsertOrUpdate(user);
            return Get(id);
        }

        public User GetByUserKey(string key, string type = "")
        {
            return Gets().FirstOrDefault(i => i.UserKey == key && i.UserType == type);
        }

        public User GetByUsername(string username, string type = "")
        {
            return Gets().FirstOrDefault(i => i.Username.ToLower() == username.ToLower() && (!string.IsNullOrEmpty(type) && i.UserType == type || true));
        }

        public ApiResponse Validate(RegisterModel model)
        {
            if (model.ConfirmPassword != model.Password)
            {
                return model.CreateResponse(false, "Mật khẩu xác nhận không khớp, Vui lòng thử lại!");
            }
            var user = Gets().FirstOrDefault(i => i.Email == model.Email);
            if (user != null)
            {
                return user.CreateResponse(false, "Email đã được sử dụng, vui lòng đăng ký bằng email khác!");
            }
            return true.CreateResponse();
        }

        public int InitRecoverPassword(string email, string url)
        {
            var u = GetByUsername(email);
            if (u == null) return 0;
            u.RecoverPasswordCode = Common.RandomString(128);
            Update(u);
            var subject = _settingService.GetValue("HNUE_ID_RECOVER_PASSWORD_SUBJECT");
            var body = _settingService.GetValue("HNUE_ID_RECOVER_PASSWORD_BODY");
            body = body.Replace("{{link}}", $"<a href='{url}?email={email}&token={u.RecoverPasswordCode}'>Bấm vào đây để khởi tạo lại mật khẩu</a>");
            _emailSender.SendAsync(subject, body, new List<EmailAddress> { new EmailAddress(email) });
            return 1;
        }

        public string RecoverPassword(string email, string token)
        {
            var u = GetByUsername(email);
            if (u != null && u.RecoverPasswordCode == token)
            {
                var pass = Common.RandomString(6);
                u.Password = Common.Md5(pass);
                Update(u);
                return pass;
            }
            return "";
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var user = Get(userId);
            user.Password = Common.Md5(newPassword);
            Update(user);
        }

        public string SetTwoFactorAuth(string email)
        {
            var key = new Random().Next(100000, 999999).ToString();
            var subject = _settingService.GetValue("HNUE_ID_TWO_FACTOR_AUTH_SUBJECT");
            var body = _settingService.GetValue("HNUE_ID_TWO_FACTOR_AUTH_BODY");
            body = body.Replace("{{key}}", key);
            _emailSender.SendAsync(subject, body, new List<EmailAddress> { new EmailAddress(email) });
            return key;
        }
    }
}