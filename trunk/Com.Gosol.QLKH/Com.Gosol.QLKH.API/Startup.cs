using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.DanhMuc;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;

namespace Com.Gosol.QLKH.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Com.Gosol.QLKH.Ultilities.SQLHelper.appConnectionStrings = configuration.GetConnectionString("DefaultConnection");
            Com.Gosol.QLKH.Ultilities.SQLHelper.backupPath = configuration.GetConnectionString("BackupPath");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
            services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            services.AddMvc()
            .AddNewtonsoftJson(
            options => options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddMvc()
             .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                                       = new DefaultContractResolver());
            //inject option appsettings in appsettings.json
            services.AddOptions();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 99999999999;
            });
            // He thong
            services.AddScoped<ILogHelper, LogHelper>();
            services.AddScoped<ISystemLogBUS, SystemLogBUS>();
            services.AddScoped<ISystemLogDAL, SystemLogDAL>();
            services.AddScoped<INguoiDungBUS, NguoiDungBUS>();
            services.AddScoped<INguoiDungDAL, NguoiDungDAL>();
            services.AddScoped<IPhanQuyenBUS, PhanQuyenBUS>();
            services.AddScoped<IPhanQuyenDAL, PhanQuyenDAL>();
            services.AddScoped<IChucNangDAL, ChucNangDAL>();
            services.AddScoped<ISystemConfigBUS, SystemConfigBUS>();
            services.AddScoped<ISystemConfigDAL, SystemConfigDAL>();
            services.AddScoped<IHeThongNguoidungBUS, HeThongNguoidungBUS>();
            services.AddScoped<IHeThongNguoiDungDAL, HeThongNguoiDungDAL>();
            services.AddScoped<IQuanTriDuLieuBUS, QuanTriDuLieuBUS>();
            services.AddScoped<IQuanTriDuLieuDAL, QuanTriDuLieuDAL>();
            services.AddScoped<IQuanTriDuLieuBUS, QuanTriDuLieuBUS>();
            services.AddScoped<IQuanTriDuLieuDAL, QuanTriDuLieuDAL>();
            services.AddScoped<IChucNangBUS, ChucNangBUS>();
            services.AddScoped<IChucNangDAL, ChucNangDAL>();
            services.AddScoped<IHuongDanSuDungBUS, HuongDanSuDungBUS>();
            services.AddScoped<IHuongDanSuDungDAL, HuongDanSuDungDAL>();

            //Danh muc
            services.AddScoped<IDanhMucChucVuBUS, DanhMucChucVuBUS>();
            services.AddScoped<IDanhMucChucVuDAL, DanhMucChucVuDAL>();
            services.AddScoped<IHeThongCanBoBUS, HeThongCanBoBUS>();
            services.AddScoped<IHeThongCanBoDAL, HeThongCanBoDAL>();
            services.AddScoped<IDanhMucDiaGioiHanhChinhDAL, DanhMucDiaGioiHanhChinhDAL>();
            services.AddScoped<IDanhMucDiaGioiHanhChinhBUS, DanhMucDiaGioiHanhChinhBUS>();
            services.AddScoped<IDanhMucCoQuanDonViBUS, DanhMucCoQuanDonViBUS>();
            services.AddScoped<IDanhMucCoQuanDonViBUS, DanhMucCoQuanDonViBUS>();
            services.AddScoped<IDanhMucLoaiTaiSanBUS, DanhMucLoaiTaiSanBUS>();
            services.AddScoped<IDanhMucLoaiTaiSanDAL, DanhMucLoaiTaiSanDAL>();
            services.AddScoped<IDanhMucNhomTaiSanBUS, DanhMucNhomTaiSanBUS>();
            services.AddScoped<IDanhMucNhomTaiSanDAL, DanhMucNhomTaiSanDAL>();
            services.AddScoped<IDanhMucTrangThaiDAL, DanhMucTrangThaiDAL>();
            services.AddScoped<IDanhMucTrangThaiBUS, DanhMucTrangThaiBUS>();
            services.AddScoped<IDanhMucCoQuanDonViDAL, DanhMucCoQuanDonViDAL>();
            services.AddScoped<IDanhMucCoQuanDonViBUS, DanhMucCoQuanDonViBUS>();
            services.AddScoped<IFileDinhKemDAL, FileDinhKemDAL>();
            services.AddScoped<IFileDinhKemBUS, FileDinhKemBUS>();
            services.AddScoped<IDanhMucBuocThucHienDAL, DanhMucBuocThucHienDAL>();
            services.AddScoped<IDanhMucBuocThucHienBUS, DanhMucBuocThucHienBUS>();
            services.AddScoped<IDanhMucBieuMauDAL, DanhMucBieuMauDAL>();
            services.AddScoped<IDanhMucBieuMauBUS, DanhMucBieuMauBUS>();
            services.AddScoped<IDanhMucLoaiKetQuaDAL, DanhMucLoaiKetQuaDAL>();
            services.AddScoped<IDanhMucLoaiKetQuaBUS, DanhMucLoaiKetQuaBUS>();
            services.AddScoped<IDanhMucLoaiHinhNghienCuuDAL, DanhMucLoaiHinhNghienCuuDAL>();
            services.AddScoped<IDanhMucLoaiHinhNghienCuuBUS, DanhMucLoaiHinhNghienCuuBUS>();

            services.AddScoped<IDeXuatDeTaiDAL, DeXuatDeTaiDAL>();
            services.AddScoped<IDeXuatDeTaiBUS, DeXuatDeTaiBUS>();
            services.AddScoped<IDanhMucCapDeTaiDAL, DanhMucCapDeTaiDAL>();
            services.AddScoped<IDanhMucCapDeTaiBUS, DanhMucCapDeTaiBUS>();
            services.AddScoped<IDanhMucLinhVucDAL, DanhMucLinhVucDAL>();
            services.AddScoped<IDanhMucLinhVucBUS, DanhMucLinhVucBUS>();
            services.AddScoped<IDeTaiDAL, DeTaiDAL>();
            services.AddScoped<IDeTaiBUS, DeTaiBUS>();
            services.AddScoped<ILyLichKhoaHocDAL, LyLichKhoaHocDAL>();
            services.AddScoped<ILyLichKhoaHocBUS, LyLichKhoaHocBUS>();
            services.AddScoped<IHoiDongDAL, HoiDongDAL>();
            services.AddScoped<IHoiDongBUS, HoiDongBUS>();
            services.AddScoped<IBaoCaoThongKeDAL, BaoCaoThongKeDAL>();
            services.AddScoped<IBaoCaoThongKeBUS, BaoCaoThongKeBUS>();
            services.AddScoped<IQuanLyThongBaoDAL, QuanLyThongBaoDAL>();
            services.AddScoped<IQuanLyThongBaoBUS, QuanLyThongBaoBUS>();
            services.AddScoped<IThuyetMinhDeTaiDAL, ThuyetMinhDeTaiDAL>();
            services.AddScoped<IThuyetMinhDeTaiBUS, ThuyetMinhDeTaiBUS>();
            ///////////////////////////////////////////
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddCors(options => options.AddPolicy("myDomain", builder =>
            {
                builder.WithOrigins("http://gocheckin.gosol.com.vn")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            var key = Encoding.ASCII.GetBytes(appSettings.AudienceSecret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver() { NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy() });
            services.AddMvc().AddNewtonsoftJson();
            services.AddHttpContextAccessor();
            services.AddScoped(sp => sp.GetRequiredService<HttpContext>().Request);
            services.AddHttpClient("HttpClientWithSSLUntrusted").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            },
                MaxRequestContentBufferSize = int.MaxValue,
            });
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowOrigin");
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
