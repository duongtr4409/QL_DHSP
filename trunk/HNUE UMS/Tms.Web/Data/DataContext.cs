using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Ums.Core.Domain.Connect;
using Ums.Core.Domain.Conversion;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.OAuth;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.Report;
using Ums.Core.Domain.Security;
using Ums.Core.Domain.System;
using Ums.Core.Domain.Task;
using Ums.Core.Domain.Users;

namespace Ums.App.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
            ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            Configuration.AutoDetectChangesEnabled = true;
        }
        public DbSet<OAuthApplication> OAuthApplication { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<SystemApplication> SystemApplication { get; set; }
        public DbSet<TaskReserved> StatisticReserved { get; set; }
        public DbSet<ConversionTeachingType> ConversionTeachingType { get; set; }
        public DbSet<SystemCategory> SystemCategory { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<Semester> Semester { get; set; }
        public DbSet<ReportType> StatisticType { get; set; }
        public DbSet<ReportData> Statistic { get; set; }
        public DbSet<Classify> Classify { get; set; }
        public DbSet<ConversionTeachingCategory> TeachingCategories { get; set; }
        public DbSet<ConversionResearchingCategory> ResearchingCategories { get; set; }
        public DbSet<ConversionWorkingCategory> WorkingCategories { get; set; }
        public DbSet<PositionGroup> PositionCategories { get; set; }
        public DbSet<ConversionStandard> Conversions { get; set; }
        public DbSet<ConversionResearching> ConversionResearchings { get; set; }
        public DbSet<ConversionTeaching> ConversionTeachings { get; set; }
        public DbSet<ConversionWorking> ConversionWorkings { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<RoleFunction> RolesFunctions { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<TaskResearching> ResearchingSchedules { get; set; }
        public DbSet<SystemSetting> Settings { get; set; }
        public DbSet<Staff> Staves { get; set; }
        public DbSet<StaffClassify> StaffClassify { get; set; }
        public DbSet<StaffPosition> StaffPositions { get; set; }
        public DbSet<TaskTeaching> TeachingSchedules { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Grade> TrainingSystems { get; set; }
        public DbSet<Year> TrainingYears { get; set; }
        public DbSet<StaffUser> Users { get; set; }
        public DbSet<TaskWorking> WorkingSchedules { get; set; }
        public DbSet<RoleGrade> RoleTrainingSystems { get; set; }
        public DbSet<Vacation> Vacation { get; set; }
        public DbSet<TitleType> TitleType { get; set; }
    }

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}