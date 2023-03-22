using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TEMIS.Model;

namespace TEMIS.CMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual HoSoGV GV { get; set; }

        public virtual HocVien HocVien { get; set; }

        public virtual GiangVien GiangVien { get; set; }

        //[Required]
        [MaxLength(16)]
        public string PrivateKey { get; set; }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserClaim>()
               .ToTable("AspNetUserClaims");
            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityUserRole>()
            .HasKey(r => new { r.UserId, r.RoleId })
            .ToTable("AspNetUserRoles");
        }
        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<HoSoGV> hoSo_GV { get; set; }

    }

    public class GiangVien
    {
        [Key]
        public long Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string HoTen { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string NoiSinh { get; set; }
        public string HoKhau { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public Nullable<int> ChucDanhId { get; set; }
        public Nullable<int> KhoaId { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
    }

    public class HocVien
    {
        [Key]
        public long Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string MaHocVien { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string NoiSinh { get; set; }
        public string HoKhau { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string DanToc { get; set; }
        public string QuocTich { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
    }
    public class HoSoGV
    {
        [Key]
        public long Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string MaGiaoVien { get; set; }
        public string MaTinh { get; set; }
        public string MaHuyen { get; set; }
        public string MaTruong { get; set; }
        public string HoVaTen { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string SoCMT { get; set; }
        public string DanToc { get; set; }
        public string NoiSinh { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public Nullable<int> SoNamKN { get; set; }
        public string MaChucDanh { get; set; }
        public string MaChucVu { get; set; }
        public string MaDiaBan { get; set; }
        public string MaTrinhDo { get; set; }
        public string MaChuyenNganh { get; set; }
        public string MaChucVuKhac { get; set; }
        public string MaCapHoc { get; set; }
        public string TenTruyCap { get; set; }
        public string MatKhau { get; set; }
        public string IDLMS { get; set; }
        public Nullable<int> RoleId { get; set; }

    }
}