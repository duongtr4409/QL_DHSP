using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using Ums.App.Base;
using Ums.App.Data;
using Ums.Services.Connect;
using Ums.Services.File;
using Ums.Services.Mailing;
using Ums.Services.OAuth;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Security;
using Ums.Services.Students;
using Ums.Services.System;
using Ums.Services.Users;

namespace Ums.App
{
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<DbContext, DataContext>();
            container.RegisterType<IStaffUserService, StaffUserService>();
            container.RegisterType<ISettingService, SettingService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IStaffService, StaffService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IEmailSender, EmailSender>();
            container.RegisterType<IApplicationService, ApplicationService>();
            container.RegisterType<IAuthenticationService, BaseAuthService>();
            container.RegisterType<IContextService, ContextService>();
            container.RegisterType<ISignInService, SignInService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IStandardStudentService, StandardStudentService>();
            container.RegisterType<ISessionService, SessionService>();
            container.RegisterType<IOAuthApplicationService, OAuthApplicationService>();
            container.RegisterType<IOAuthTokenService, OAuthTokenService>();
            container.RegisterType<INoticeService, NoticeService>();
            container.RegisterType<IFeedbackService, FeedbackService>();
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IFileContentService, FileContentService>();
            container.RegisterType<IFileAccessService, FileAccessService>();
            container.RegisterType<IMasterStudentService, MasterStudentService>();
        }

        public static T Resolve<T>()
        {
            return Container.Value.Resolve<T>();
        }
    }
}
