using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Ums.Core.Domain.Connect;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.File;
using Ums.Core.Domain.OAuth;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.System;
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
        public DbSet<FileAccess> FileAccess { get; set; }
        public DbSet<FileContent> FileContent { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<OAuthToken> OAuthToken { get; set; }
        public DbSet<OAuthApplication> OAuthApplication { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<SystemSetting> SystemSetting { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Staff> Staves { get; set; }
        public DbSet<StaffPosition> StaffPositions { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Grade> TrainingSystems { get; set; }
        public DbSet<Year> TrainingYears { get; set; }
        public DbSet<StaffUser> Users { get; set; }
        public DbSet<User> User { get; set; }
    }

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}