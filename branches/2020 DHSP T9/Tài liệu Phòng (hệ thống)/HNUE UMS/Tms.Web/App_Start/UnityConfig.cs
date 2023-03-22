using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using Ums.App.Base;
using Ums.App.Data;
using Ums.App.System;
using Ums.Services.Calculate;
using Ums.Services.Connect;
using Ums.Services.Conversion;
using Ums.Services.Conversion.Type;
using Ums.Services.Data;
using Ums.Services.Mailing;
using Ums.Services.OAuth;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Report;
using Ums.Services.Security;
using Ums.Services.Statistic;
using Ums.Services.Students;
using Ums.Services.Sync;
using Ums.Services.System;
using Ums.Services.Task;
using Ums.Services.Users;
using IResearchingService = Ums.Services.Conversion.Category.IResearchingService;
using ITeachingService = Ums.Services.Conversion.ITeachingService;
using IWorkingService = Ums.Services.Conversion.IWorkingService;
using ResearchingService = Ums.Services.Conversion.Category.ResearchingService;
using TeachingService = Ums.Services.Conversion.TeachingService;
using WorkingService = Ums.Services.Conversion.WorkingService;

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
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IFunctionService, FunctionService>();
            container.RegisterType<ISignInService, SignInService>();
            container.RegisterType<IAuthenticationService, BaseAuthService>();
            container.RegisterType<IResearchingService, ResearchingService>();
            container.RegisterType<ISettingService, SettingService>();
            container.RegisterType<IStandardService, StandardService>();
            container.RegisterType<ITeachingTypeService, TeachingTypeService>();
            container.RegisterType<Services.Conversion.IResearchingService, Services.Conversion.ResearchingService>();
            container.RegisterType<ITeachingService, TeachingService>();
            container.RegisterType<IWorkingService, WorkingService>();
            container.RegisterType<Services.Conversion.Category.IWorkingService, Services.Conversion.Category.WorkingService>();
            container.RegisterType<Services.Conversion.Category.ITeachingService, Services.Conversion.Category.TeachingService>();
            container.RegisterType<IDegreeService, DegreeService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IPositionGroupService, PositionGroupService>();
            container.RegisterType<IPositionService, PositionService>();
            container.RegisterType<IStaffPositionService, StaffPositionService>();
            container.RegisterType<IStaffService, StaffService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<ITitleService, TitleService>();
            container.RegisterType<IYearService, YearService>();
            container.RegisterType<IGradeService, GradeService>();
            container.RegisterType<IRoleFunctionService, RoleFunctionService>();
            container.RegisterType<IRoleGradeService, RoleGradeService>();
            container.RegisterType<IUserRoleService, UserRoleService>();
            container.RegisterType<IVacationService, VacationService>();
            container.RegisterType<IClassifyService, ClassifyService>();
            container.RegisterType<IVacationTypeService, VacationTypeService>();
            container.RegisterType<IEmailSender, EmailSender>();
            container.RegisterType<IContextService, ContextService>();
            container.RegisterType<ISemesterService, SemesterService>();
            container.RegisterType<ITitleTypeService, TitleTypeService>();
            container.RegisterType<INoticeService, NoticeService>();
            container.RegisterType<IFeedbackService, FeedbackService>();
            container.RegisterType<ITeachingSyncService, TeachingSyncService>();
            container.RegisterType<IStaffClassifyService, StaffClassifyService>();
            container.RegisterType<IApplicationService, ApplicationService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ISessionService, SessionService>();
            container.RegisterType<IStandardStudentService, StandardStudentService>();
            container.RegisterType<IUserGroupService, UserGroupService>();
            container.RegisterType<IUserTypeService, UserTypeService>();
            container.RegisterType<ISystemLogService, SystemLogService>();
            container.RegisterType<IApplicationContext, ApiContextController>();
            container.RegisterType<IOAuthApplicationService, OAuthApplicationService>();
            container.RegisterType<IStatisticService, StatisticService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<ITypeService, TypeService>();
            container.RegisterType<Ums.Services.Task.ResearchingService, Ums.Services.Task.ResearchingService>();
            container.RegisterType<ITaskReservedService, TaskReservedService>();
            container.RegisterType<Ums.Services.Task.ITeachingService, Ums.Services.Task.TeachingService>();
            container.RegisterType<Ums.Services.Task.IWorkingService, Ums.Services.Task.WorkingService>();
            container.RegisterType<IStaffCalculate, StaffCalculate>();
            container.RegisterType<IResearchingSyncService, ResearchingSyncService>();
        }

        public static T Resolve<T>()
        {
            return Container.Value.Resolve<T>();
        }
    }
}
