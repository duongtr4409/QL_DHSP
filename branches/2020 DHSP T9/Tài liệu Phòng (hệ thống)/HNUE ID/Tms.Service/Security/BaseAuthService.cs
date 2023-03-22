using Hnue.Helper;
using System;
using System.Configuration;
using System.DirectoryServices;
using Ums.Core.Domain.Users;
using Ums.Models.Security;
using Ums.Services.Students;
using Ums.Services.System;
using Ums.Services.Users;

namespace Ums.Services.Security
{
    public class BaseAuthService : IAuthenticationService
    {
        private readonly IStaffUserService _staffUserService;
        private readonly IUserService _userService;
        private readonly IStandardStudentService _standardStudentService;
        private readonly IMasterStudentService _masterStudentService;
        private readonly ISettingService _settingService;
        public BaseAuthService(IStaffUserService systemUser, IUserService userService, IStandardStudentService standardStudentService, ISettingService settingService, IMasterStudentService masterStudentService)
        {
            _staffUserService = systemUser;
            _userService = userService;
            _standardStudentService = standardStudentService;
            _settingService = settingService;
            _masterStudentService = masterStudentService;
        }

        public User Validate(string username, string password, string type)
        {
            int userId = 0;
            switch (type)
            {
                case "MASTER_STUDENT":
                    {
                        var u = _masterStudentService.GetStudent(username);
                        if (u != null && u.Code == password)
                        {
                            var key = u.Id.ToString();
                            var user = _userService.GetByUserKey(key, "STANDARD_STUDENT") ?? new User();
                            user.Birthday = u.NgaySinh;
                            user.Email = u.Email;
                            user.Name = u.Ho + " " + u.Ten;
                            user.UserKey = key;
                            user.Password = u.Code;
                            user.Username = u.MaHv;
                            user.UserType = "MASTER_STUDENT";
                            user.Address = u.DiaChi;
                            userId = _userService.InsertOrUpdate(user);
                        }
                    }
                    break;
                case "STANDARD_STUDENT":
                    {
                        var u = _standardStudentService.GetStudent(username);
                        if (u != null && u.Password == password)
                        {
                            var key = u.Id.ToString();
                            var user = _userService.GetByUserKey(key, "STANDARD_STUDENT") ?? new User();
                            user.Birthday = u.Birthday;
                            user.Email = u.Email;
                            user.Name = u.Name;
                            user.UserKey = key;
                            user.Password = u.Password;
                            user.Username = u.Username;
                            user.UserType = "STANDARD_STUDENT";
                            user.Address = u.Address;
                            userId = _userService.InsertOrUpdate(user);
                        }
                    }
                    break;
                case "STAFF":
                    {
                        var u = _staffUserService.Get(username);
                        if (u != null && u.Password == Common.Md5(password))
                        {
                            var key = u.Id.ToString();
                            var user = _userService.GetByUserKey(key, "STAFF") ?? new User();
                            user.Birthday = u.Staff.Birthday;
                            user.Email = u.Email;
                            user.Name = u.Staff.Name;
                            user.UserKey = key;
                            user.Password = u.Password;
                            user.Username = u.Username;
                            user.UserType = "STAFF";
                            user.StaffId = u.StaffId;
                            userId = _userService.InsertOrUpdate(user);
                        }
                    }
                    break;
                case "LDAP":
                    {
                        var path = _settingService.GetValue("LDAP_PATH");
                        DirectoryEntry RootEntry = new DirectoryEntry(path, _settingService.GetValue("LDAP_USERNAME"), _settingService.GetValue("LDAP_PASSWORD"), AuthenticationTypes.None);
                        try
                        {
                            var userSearch = new DirectorySearcher(RootEntry) { Filter = "uid=" + username }.FindOne().GetDirectoryEntry();
                            var ldapUser = new DirectoryEntry(path, userSearch.Path.Split('/')[3], password, AuthenticationTypes.None);
                            var loginUser = new DirectorySearcher(ldapUser) { Filter = userSearch.Name }.FindOne().GetDirectoryEntry();
                            if (loginUser.Name != null)
                            {
                                var user = _userService.GetByUsername(loginUser.Name, "LDAP") ?? new User();
                                var key = loginUser.Name.ToString();
                                user.UserKey = key;
                                user.Password = password;
                                user.Username = loginUser.Name;
                                user.UserType = "LDAP";
                                user.Name = username;
                                userId = _userService.InsertOrUpdate(user);
                            }
                        }
                        catch
                        {
                        }
                    }
                    break;
                case "GUEST":
                    {
                        var u = _userService.GetByUsername(username, "GUEST");
                        if (u != null && u.Password == Common.Md5(password))
                        {
                            userId = u.Id;
                        }
                    }
                    break;
            }
            return _userService.Get(userId);
        }

        public int Validate(GoogleTokenModel model)
        {
            if (model.error == "invalid_token")
            {
                return 0;
            }
            var user = _userService.GetByUserKey(model.sub, "GOOGLE") ?? new User();
            user.Email = model.email;
            user.Name = model.name;
            user.UserKey = model.sub;
            user.Username = model.email;
            user.UserType = "GOOGLE";
            user.Avatar = model.picture;
            user.Password = Common.Md5(Common.RandomString(128));
            return _userService.InsertOrUpdate(user);
        }

        public int Validate(FacebookTokenModel model)
        {
            var user = _userService.GetByUserKey(model.id, "FACEBOOK") ?? new User();
            user.Email = model.email;
            user.Name = model.name;
            user.UserKey = model.id;
            user.Username = model.email;
            user.UserType = "FACEBOOK";
            user.Avatar = model.picture.data.url;
            user.Password = Common.Md5(Common.RandomString(128));
            return _userService.InsertOrUpdate(user);
        }
    }
}