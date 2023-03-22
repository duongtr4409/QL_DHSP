using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Microsoft.Extensions.Options;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.Ultilities;
using System.Security.Claims;
using Com.Gosol.QLKH.API.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Com.Gosol.QLKH.Security;
using Microsoft.Extensions.Logging;
using Com.Gosol.TDNV.Ultilities;
using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.Models.DanhMuc;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;
using Castle.Core.Internal;

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    [Route("api/v1/Nguoidung")]
    [ApiController]
    public class NguoidungController : ControllerBase
    {
        private IOptions<AppSettings> _AppSettings;
        private INguoiDungBUS _NguoiDungBUS;
        private IPhanQuyenBUS _PhanQuyenBUS;
        private ILogHelper _ILogHelper;
        private ILogger logger;
        private RestShapAPIInCore rsApiInCore;
        private IMemoryCache _cache;
        private ISystemConfigBUS _SystemConfigBUS;
        public NguoidungController(ISystemConfigBUS _SystemConfig, IMemoryCache memoryCache, IOptions<AppSettings> Settings, INguoiDungBUS NguoiDungBUS, IPhanQuyenBUS PhanQuyenBUS, ILogHelper LogHelper, ILogger<NguoidungController> logger)
        {
            _AppSettings = Settings;
            _NguoiDungBUS = NguoiDungBUS;
            _PhanQuyenBUS = PhanQuyenBUS;
            _ILogHelper = LogHelper;
            this.logger = logger;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            _cache = memoryCache;
            this._SystemConfigBUS = _SystemConfig;
        }

        [Route("DangNhap")]
        [HttpPost]
        public IActionResult Login(LoginModel User)
        {
            try
            {
                NguoiDungModel NguoiDung = new NguoiDungModel();
                var NguoiDungInCore = this.rsApiInCore.core_Login(User.UserName, User.Password);
                if (NguoiDungInCore.Id > 0)
                {
                    var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                    NguoiDung.NguoiDungID = NguoiDungInCore.Id;
                    NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                    NguoiDung.TenCanBo = NguoiDungInCore.Name;
                    NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                    NguoiDung.CoQuanID = ttCanBo.DepartmentId;
                }
                else
                {
                    //string Password = Cryptor.EncryptPasswordUser(User.UserName.Trim().ToLower(), User.Password);
                    string Password = Cryptor.Encrypt(User.Password.Trim(), true);
                    _NguoiDungBUS.VerifyUser(User.UserName.Trim(), Password, ref NguoiDung);
                }
                if (NguoiDung != null && NguoiDung.NguoiDungID > 0)
                {
                    Task.Run(() => _ILogHelper.Log(NguoiDung.CanBoID, "Đăng nhập hệ thống", (int)EnumLogType.DangNhap));
                    var claims = new List<Claim>();
                    var ListChucNang = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDung.NguoiDungID);
                    //string ClaimFull = "," + string.Join(",", ListChucNang.Where(t => t.Quyen == (int)AccessLevel.FullAccess).Select(t => new { t.ChucNangID }).ToList()) + ",";

                    string ClaimRead = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimCreate = "," + string.Join(",", ListChucNang.Where(t => t.Them == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimEdit = "," + string.Join(",", ListChucNang.Where(t => t.Sua == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimDelete = "," + string.Join(",", ListChucNang.Where(t => t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimFullAccess = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1 && t.Them == 1 && t.Sua == 1 && t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";

                    //claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFull));
                    claims.Add(new Claim(PermissionLevel.READ, ClaimRead));
                    claims.Add(new Claim(PermissionLevel.CREATE, ClaimCreate));
                    claims.Add(new Claim(PermissionLevel.EDIT, ClaimEdit));
                    claims.Add(new Claim(PermissionLevel.DELETE, ClaimDelete));
                    claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFullAccess));

                    claims.Add(new Claim("CanBoID", NguoiDung?.CanBoID.ToString()));
                    claims.Add(new Claim("NguoiDungID", NguoiDung?.NguoiDungID.ToString()));
                    claims.Add(new Claim("CoQuanID", NguoiDung?.CoQuanID.ToString()));
                    claims.Add(new Claim("TenNguoiDung", NguoiDung?.TenNguoiDung.ToString()));
                    //claims.Add(new Claim("CapCoQuan", NguoiDung?.CapCoQuan.ToString()));
                    claims.Add(new Claim("VaiTro", NguoiDung?.VaiTro.ToString()));
                    claims.Add(new Claim("expires_at", Utils.ConvertToDateTime(DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire).ToString(), DateTime.Now.Date).ToString()));
                    claims.Add(new Claim("TenCanBo", NguoiDung?.TenCanBo.ToString()));
                    //claims.Add(new Claim("expires_at", new DateTime(2020,01,07,13,45,00).ToString()));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_AppSettings.Value.AudienceSecret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire),
                        //new DateTime(2020, 01, 07, 13, 45, 00),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        //,Issuer = _AppSettings.Value.ApiUrl
                        //, Audience = _AppSettings.Value.AudienceSecret

                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    NguoiDung.Token = tokenHandler.WriteToken(token);
                    NguoiDung.expires_at = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire);
                    DanhMucCoQuanDonViPartialNew CoQuanInfo = new DanhMucCoQuanDonViDAL().GetByID(NguoiDung.CoQuanID);
                    return Ok(new
                    {
                        Status = 1,
                        User = NguoiDung,
                        ListRole = ListChucNang
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Status = -1,
                        Message = Constant.NOT_ACCOUNT
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message, "Đăng nhập hệ thống");
                throw;
            }


        }

        [Route("DangNhapSSO")]
        [HttpPost]
        public IActionResult LoginSSO(LoginModel User)
        {
            try
            {
                NguoiDungModel NguoiDung = new NguoiDungModel();
                if (User.AccessToken != null && User.AccessToken != "")
                {
                    //Đăng nhập qua SS0
                    var NguoiDungInCore = this.rsApiInCore.core_LoginSSO(User.AccessToken);
                    if (NguoiDungInCore.Id > 0)
                    {
                        //var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                        //NguoiDung.NguoiDungID = NguoiDungInCore.Id;  
                        NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                        NguoiDung.TenCanBo = NguoiDungInCore.Name;
                        NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                        //NguoiDung.CoQuanID = ttCanBo.DepartmentId;


                        int NguoiDungID = Utils.ConvertToInt32(NguoiDungInCore.UserKey, 0);
                        NguoiDung.NguoiDungID = NguoiDungID;
                        var ttCanBo = this.rsApiInCore.core_getusers(0);
                        foreach (var item in ttCanBo)
                        {
                            if (item.Id == NguoiDungID)
                            {
                                NguoiDung.CoQuanID = item.DepartmentId;
                                NguoiDung.CanBoID = item.StaffId;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var NguoiDungInCore = this.rsApiInCore.core_Login(User.UserName, User.Password);
                    if (NguoiDungInCore.Id > 0)
                    {
                        var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                        NguoiDung.NguoiDungID = NguoiDungInCore.Id;
                        NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                        NguoiDung.TenCanBo = NguoiDungInCore.Name;
                        NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                        NguoiDung.CoQuanID = ttCanBo.DepartmentId;
                    }
                    //else
                    //{
                    //    //string Password = Cryptor.EncryptPasswordUser(User.UserName.Trim().ToLower(), User.Password);
                    //    string Password = Cryptor.Encrypt(User.Password.Trim(), true);
                    //    _NguoiDungBUS.VerifyUser(User.UserName.Trim(), Password, ref NguoiDung);
                    //}
                }

                if (NguoiDung != null && NguoiDung.NguoiDungID > 0)
                {
                    // add người dùng vào nhóm nhà khoa học nếu chưa có
                    var idNhomNguoiDungNKH = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_NKH").ConfigValue, 0);
                    var dsNguoiDungTrongNhomNKH = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomNguoiDungNKH);
                    if (!dsNguoiDungTrongNhomNKH.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                    {
                        var ndnnd = new NguoiDungNhomNguoiDungModel();
                        ndnnd.NguoiDungID = NguoiDung.NguoiDungID;
                        ndnnd.CoQuanID = NguoiDung.CoQuanID;
                        ndnnd.NhomNguoiDungID = idNhomNguoiDungNKH;
                        var result = _PhanQuyenBUS.NguoiDung_NhomNguoiDung_Insert(ndnnd);
                    }

                    Task.Run(() => _ILogHelper.Log(NguoiDung.CanBoID, "Đăng nhập hệ thống", (int)EnumLogType.DangNhap));
                    var claims = new List<Claim>();
                    var ListChucNang = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDung.NguoiDungID);
                    //string ClaimFull = "," + string.Join(",", ListChucNang.Where(t => t.Quyen == (int)AccessLevel.FullAccess).Select(t => new { t.ChucNangID }).ToList()) + ",";

                    string ClaimRead = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimCreate = "," + string.Join(",", ListChucNang.Where(t => t.Them == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimEdit = "," + string.Join(",", ListChucNang.Where(t => t.Sua == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimDelete = "," + string.Join(",", ListChucNang.Where(t => t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimFullAccess = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1 && t.Them == 1 && t.Sua == 1 && t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";

                    //claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFull));
                    claims.Add(new Claim(PermissionLevel.READ, ClaimRead));
                    claims.Add(new Claim(PermissionLevel.CREATE, ClaimCreate));
                    claims.Add(new Claim(PermissionLevel.EDIT, ClaimEdit));
                    claims.Add(new Claim(PermissionLevel.DELETE, ClaimDelete));
                    claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFullAccess));

                    claims.Add(new Claim("AccessToken", User.AccessToken));
                    claims.Add(new Claim("CanBoID", NguoiDung?.CanBoID.ToString()));
                    claims.Add(new Claim("NguoiDungID", NguoiDung?.NguoiDungID.ToString()));
                    claims.Add(new Claim("CoQuanID", NguoiDung?.CoQuanID.ToString()));
                    claims.Add(new Claim("TenNguoiDung", NguoiDung?.TenNguoiDung.ToString()));
                    //claims.Add(new Claim("CapCoQuan", NguoiDung?.CapCoQuan.ToString()));
                    claims.Add(new Claim("VaiTro", NguoiDung?.VaiTro.ToString()));
                    claims.Add(new Claim("expires_at", Utils.ConvertToDateTime(DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire).ToString(), DateTime.Now.Date).ToString()));
                    claims.Add(new Claim("TenCanBo", NguoiDung?.TenCanBo.ToString()));
                    //claims.Add(new Claim("expires_at", new DateTime(2020,01,07,13,45,00).ToString()));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_AppSettings.Value.AudienceSecret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire),
                        //new DateTime(2020, 01, 07, 13, 45, 00),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        //,Issuer = _AppSettings.Value.ApiUrl
                        //, Audience = _AppSettings.Value.AudienceSecret

                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    NguoiDung.Token = tokenHandler.WriteToken(token);
                    NguoiDung.expires_at = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire);
                    DanhMucCoQuanDonViPartialNew CoQuanInfo = new DanhMucCoQuanDonViDAL().GetByID(NguoiDung.CoQuanID);

                    // map chức năng theo role
                    var ListRoles = new List<ChucNangPartialModel>();
                    // nếu là admin
                    if (_PhanQuyenBUS.CheckAdmin(NguoiDung.NguoiDungID))
                    {
                        var roleAdmin = new ChucNangPartialModel();
                        roleAdmin.RoleName = "Admin";
                        roleAdmin.Role = new List<ChucNangModel>();
                        roleAdmin.Role.AddRange(ListChucNang.Where(x => x.ChucNangID >= 200 && x.ChucNangID < 300).ToList());
                        roleAdmin.RoleID = 0;
                        ListRoles.Add(roleAdmin);
                        ListChucNang = roleAdmin.Role;
                    }
                    else
                    {
                        var idNhomNKH = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_NKH").ConfigValue, 0);
                        var listNguoiDungTrongNhomNKH = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomNKH);
                        var dsChucNangNKH = new List<ChucNangModel>();
                        if (listNguoiDungTrongNhomNKH.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                        {
                            dsChucNangNKH = _PhanQuyenBUS.ChucNang_GetQuyenByNhomNguoiDungID(idNhomNKH);
                        }

                        var idNhomTruongKhoa = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_TRUONG_KHOA").ConfigValue, 0);
                        var listNguoiDungTrongNhomTruongKhoa = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomTruongKhoa);
                        var dsChucNangTruongKhoa = new List<ChucNangModel>();
                        if (listNguoiDungTrongNhomTruongKhoa.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                        {
                            dsChucNangTruongKhoa = _PhanQuyenBUS.ChucNang_GetQuyenByNhomNguoiDungID(idNhomTruongKhoa);
                        }

                        var idNhomQLKH = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_QLKH").ConfigValue, 0);
                        var listNguoiDungTrongNhomQLKH = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomQLKH);
                        var dsChucNangQLKH = new List<ChucNangModel>();
                        if (listNguoiDungTrongNhomQLKH.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                        {
                            dsChucNangQLKH = _PhanQuyenBUS.ChucNang_GetQuyenByNhomNguoiDungID(idNhomQLKH);
                        }

                        if (dsChucNangNKH != null && dsChucNangNKH.Count > 0)
                        {
                            var chucNang = new ChucNangPartialModel();
                            chucNang.RoleName = "NKH";
                            chucNang.RoleID = idNhomNKH;
                            chucNang.Role = new List<ChucNangModel>();
                            chucNang.Role.AddRange(dsChucNangNKH.Where(x => ListChucNang.Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
                            ListRoles.Add(chucNang);
                        }
                        if (dsChucNangTruongKhoa != null && dsChucNangTruongKhoa.Count > 0)
                        {
                            var chucNang1 = new ChucNangPartialModel();
                            chucNang1.RoleName = "TRUONGKHOA";
                            chucNang1.RoleID = idNhomTruongKhoa;
                            chucNang1.Role = new List<ChucNangModel>();
                            chucNang1.Role.AddRange(dsChucNangTruongKhoa.Where(x => ListChucNang .Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
                            ListRoles.Add(chucNang1);
                        }
                        if (dsChucNangQLKH != null && dsChucNangQLKH.Count > 0)
                        {
                            var chucNang2 = new ChucNangPartialModel();
                            chucNang2.RoleName = "QLKH";
                            chucNang2.RoleID = idNhomQLKH;
                            chucNang2.Role = new List<ChucNangModel>();
                            chucNang2.Role.AddRange(dsChucNangQLKH.Where(x => ListChucNang .Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
                            ListRoles.Add(chucNang2);
                        }
                    }

                    return Ok(new
                    {
                        Status = 1,
                        User = NguoiDung,
                        ListRole = ListChucNang,
                        Roles = ListRoles
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Status = -1,
                        Message = (User.AccessToken != null && User.AccessToken != "") ? Constant.NOT_ACCOUNT_SSO : Constant.NOT_ACCOUNT
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message, "Đăng nhập hệ thống");
                throw;
            }


        }

        [Route("DangNhapSSO_fake")]
        [HttpPost]
        public IActionResult LoginSSO_fake(LoginModel User)
        {
            try
            {
                NguoiDungModel NguoiDung = new NguoiDungModel();
                if (User.AccessToken.IsNullOrEmpty() || User.AccessToken.Equals("?ADMIN"))
                {
                    var NguoiDungInCore = this.rsApiInCore.DuowngToraFake_Core_LoginSSO(User.AccessToken);
                    if (NguoiDungInCore.Id > 0)
                    {
                        //var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                        //NguoiDung.NguoiDungID = NguoiDungInCore.Id;  
                        NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                        NguoiDung.TenCanBo = NguoiDungInCore.Name;
                        NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                        //NguoiDung.CoQuanID = ttCanBo.DepartmentId;


                        int NguoiDungID = Utils.ConvertToInt32(NguoiDungInCore.UserKey, 4177);
                        NguoiDung.NguoiDungID = NguoiDungID;
                        var ttCanBo = this.rsApiInCore.DuowngToraFake_Core_getusers(0);
                        foreach (var item in ttCanBo)
                        {
                            if (item.Id == NguoiDungID)
                            {
                                NguoiDung.CoQuanID = item.DepartmentId;
                                NguoiDung.CanBoID = item.StaffId;
                                break;
                            }
                        }
                    }
                }
                else if (User.AccessToken.Equals("?QLKH"))
                {
                    var NguoiDungInCore = this.rsApiInCore.DuowngToraFake_Core_LoginSSO(User.AccessToken);
                    if (NguoiDungInCore.Id > 0)
                    {
                        //var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                        //NguoiDung.NguoiDungID = NguoiDungInCore.Id;  
                        NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                        NguoiDung.TenCanBo = NguoiDungInCore.Name;
                        NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                        //NguoiDung.CoQuanID = ttCanBo.DepartmentId;


                        int NguoiDungID = Utils.ConvertToInt32(NguoiDungInCore.UserKey, 60);
                        NguoiDung.NguoiDungID = NguoiDungID;
                        var ttCanBo = this.rsApiInCore.DuowngToraFake_Core_getusers(0);
                        foreach (var item in ttCanBo)
                        {
                            if (item.Id == NguoiDungID)
                            {
                                NguoiDung.CoQuanID = item.DepartmentId;
                                NguoiDung.CanBoID = item.StaffId;
                                break;
                            }
                        }
                    }
                }
                else if (User.AccessToken.Equals("?NKH"))
                {
                    var NguoiDungInCore = this.rsApiInCore.DuowngToraFake_Core_LoginSSO(User.AccessToken);
                    if (NguoiDungInCore.Id > 0)
                    {
                        //var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                        //NguoiDung.NguoiDungID = NguoiDungInCore.Id;  
                        NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                        NguoiDung.TenCanBo = NguoiDungInCore.Name;
                        NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                        //NguoiDung.CoQuanID = ttCanBo.DepartmentId;


                        int NguoiDungID = Utils.ConvertToInt32(NguoiDungInCore.UserKey, 58);
                        NguoiDung.NguoiDungID = NguoiDungID;
                        var ttCanBo = this.rsApiInCore.DuowngToraFake_Core_getusers(0);
                        foreach (var item in ttCanBo)
                        {
                            if (item.Id == NguoiDungID)
                            {
                                NguoiDung.CoQuanID = item.DepartmentId;
                                NguoiDung.CanBoID = item.StaffId;
                                break;
                            }
                        }
                    }
                }
                if (User.AccessToken != null && User.AccessToken != "")
                {
                    //Đăng nhập qua SS0
                    var NguoiDungInCore = this.rsApiInCore.core_LoginSSO(User.AccessToken);
                    if (NguoiDungInCore.Id > 0)
                    {
                        //var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                        //NguoiDung.NguoiDungID = NguoiDungInCore.Id;  
                        NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                        NguoiDung.TenCanBo = NguoiDungInCore.Name;
                        NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                        //NguoiDung.CoQuanID = ttCanBo.DepartmentId;


                        int NguoiDungID = Utils.ConvertToInt32(NguoiDungInCore.UserKey, 0);
                        NguoiDung.NguoiDungID = NguoiDungID;
                        var ttCanBo = this.rsApiInCore.core_getusers(0);
                        foreach (var item in ttCanBo)
                        {
                            if (item.Id == NguoiDungID)
                            {
                                NguoiDung.CoQuanID = item.DepartmentId;
                                NguoiDung.CanBoID = item.StaffId;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var NguoiDungInCore = this.rsApiInCore.core_Login(User.UserName, User.Password);
                    if (NguoiDungInCore.Id > 0)
                    {
                        var ttCanBo = this.rsApiInCore.core_getstaff(NguoiDungInCore.StaffId);
                        NguoiDung.NguoiDungID = NguoiDungInCore.Id;
                        NguoiDung.TenNguoiDung = NguoiDungInCore.Username;
                        NguoiDung.TenCanBo = NguoiDungInCore.Name;
                        NguoiDung.CanBoID = NguoiDungInCore.StaffId;
                        NguoiDung.CoQuanID = ttCanBo.DepartmentId;
                    }
                    //else
                    //{
                    //    //string Password = Cryptor.EncryptPasswordUser(User.UserName.Trim().ToLower(), User.Password);
                    //    string Password = Cryptor.Encrypt(User.Password.Trim(), true);
                    //    _NguoiDungBUS.VerifyUser(User.UserName.Trim(), Password, ref NguoiDung);
                    //}
                }

                if (NguoiDung != null && NguoiDung.NguoiDungID > 0)
                {
                    // add người dùng vào nhóm nhà khoa học nếu chưa có
                    var idNhomNguoiDungNKH = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_NKH").ConfigValue, 0);
                    var dsNguoiDungTrongNhomNKH = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomNguoiDungNKH);
                    if (!dsNguoiDungTrongNhomNKH.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                    {
                        var ndnnd = new NguoiDungNhomNguoiDungModel();
                        ndnnd.NguoiDungID = NguoiDung.NguoiDungID;
                        ndnnd.CoQuanID = NguoiDung.CoQuanID;
                        ndnnd.NhomNguoiDungID = idNhomNguoiDungNKH;
                        var result = _PhanQuyenBUS.NguoiDung_NhomNguoiDung_Insert(ndnnd);
                    }

                    Task.Run(() => _ILogHelper.Log(NguoiDung.CanBoID, "Đăng nhập hệ thống", (int)EnumLogType.DangNhap));
                    var claims = new List<Claim>();
                    var ListChucNang = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDung.NguoiDungID);
                    //string ClaimFull = "," + string.Join(",", ListChucNang.Where(t => t.Quyen == (int)AccessLevel.FullAccess).Select(t => new { t.ChucNangID }).ToList()) + ",";

                    string ClaimRead = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimCreate = "," + string.Join(",", ListChucNang.Where(t => t.Them == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimEdit = "," + string.Join(",", ListChucNang.Where(t => t.Sua == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimDelete = "," + string.Join(",", ListChucNang.Where(t => t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    string ClaimFullAccess = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1 && t.Them == 1 && t.Sua == 1 && t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";

                    //claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFull));
                    claims.Add(new Claim(PermissionLevel.READ, ClaimRead));
                    claims.Add(new Claim(PermissionLevel.CREATE, ClaimCreate));
                    claims.Add(new Claim(PermissionLevel.EDIT, ClaimEdit));
                    claims.Add(new Claim(PermissionLevel.DELETE, ClaimDelete));
                    claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFullAccess));

                    claims.Add(new Claim("AccessToken", User.AccessToken));
                    claims.Add(new Claim("CanBoID", NguoiDung?.CanBoID.ToString()));
                    claims.Add(new Claim("NguoiDungID", NguoiDung?.NguoiDungID.ToString()));
                    claims.Add(new Claim("CoQuanID", NguoiDung?.CoQuanID.ToString()));
                    claims.Add(new Claim("TenNguoiDung", NguoiDung?.TenNguoiDung.ToString()));
                    //claims.Add(new Claim("CapCoQuan", NguoiDung?.CapCoQuan.ToString()));
                    claims.Add(new Claim("VaiTro", NguoiDung?.VaiTro.ToString()));
                    claims.Add(new Claim("expires_at", Utils.ConvertToDateTime(DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire).ToString(), DateTime.Now.Date).ToString()));
                    claims.Add(new Claim("TenCanBo", NguoiDung?.TenCanBo.ToString()));
                    //claims.Add(new Claim("expires_at", new DateTime(2020,01,07,13,45,00).ToString()));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_AppSettings.Value.AudienceSecret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire),
                        //new DateTime(2020, 01, 07, 13, 45, 00),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        //,Issuer = _AppSettings.Value.ApiUrl
                        //, Audience = _AppSettings.Value.AudienceSecret

                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    NguoiDung.Token = tokenHandler.WriteToken(token);
                    NguoiDung.expires_at = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire);
                    DanhMucCoQuanDonViPartialNew CoQuanInfo = new DanhMucCoQuanDonViDAL().GetByID(NguoiDung.CoQuanID);

                    // map chức năng theo role
                    var ListRoles = new List<ChucNangPartialModel>();
                    // nếu là admin
                    if (_PhanQuyenBUS.CheckAdmin(NguoiDung.NguoiDungID))
                    {
                        var roleAdmin = new ChucNangPartialModel();
                        roleAdmin.RoleName = "Admin";
                        roleAdmin.Role = new List<ChucNangModel>();
                        roleAdmin.Role.AddRange(ListChucNang.Where(x => x.ChucNangID >= 200 && x.ChucNangID < 300).ToList());
                        roleAdmin.RoleID = 0;
                        ListRoles.Add(roleAdmin);
                        ListChucNang = roleAdmin.Role;
                    }
                    else
                    {
                        var idNhomNKH = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_NKH").ConfigValue, 0);
                        var listNguoiDungTrongNhomNKH = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomNKH);
                        var dsChucNangNKH = new List<ChucNangModel>();
                        if (listNguoiDungTrongNhomNKH.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                        {
                            dsChucNangNKH = _PhanQuyenBUS.ChucNang_GetQuyenByNhomNguoiDungID(idNhomNKH);
                        }

                        var idNhomTruongKhoa = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_TRUONG_KHOA").ConfigValue, 0);
                        var listNguoiDungTrongNhomTruongKhoa = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomTruongKhoa);
                        var dsChucNangTruongKhoa = new List<ChucNangModel>();
                        if (listNguoiDungTrongNhomTruongKhoa.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                        {
                            dsChucNangTruongKhoa = _PhanQuyenBUS.ChucNang_GetQuyenByNhomNguoiDungID(idNhomTruongKhoa);
                        }

                        var idNhomQLKH = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_QLKH").ConfigValue, 0);
                        var listNguoiDungTrongNhomQLKH = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomQLKH);
                        var dsChucNangQLKH = new List<ChucNangModel>();
                        if (listNguoiDungTrongNhomQLKH.Select(x => x.NguoiDungID).ToList().Contains(NguoiDung.NguoiDungID))
                        {
                            dsChucNangQLKH = _PhanQuyenBUS.ChucNang_GetQuyenByNhomNguoiDungID(idNhomQLKH);
                        }
                        if (dsChucNangNKH != null && dsChucNangNKH.Count > 0)
                        {
                            var chucNang = new ChucNangPartialModel();
                            chucNang.RoleName = "NKH";
                            chucNang.RoleID = idNhomNKH;
                            chucNang.Role = new List<ChucNangModel>();
                            chucNang.Role.AddRange(dsChucNangNKH.Where(x => ListChucNang.Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
                            ListRoles.Add(chucNang);
                            ListChucNang.AddRange(chucNang.Role);
                        }
                        if (dsChucNangTruongKhoa != null && dsChucNangTruongKhoa.Count > 0)
                        {
                            var chucNang1 = new ChucNangPartialModel();
                            chucNang1.RoleName = "TRUONGKHOA";
                            chucNang1.RoleID = idNhomTruongKhoa;
                            chucNang1.Role = new List<ChucNangModel>();
                            chucNang1.Role.AddRange(dsChucNangQLKH.Where(x => ListChucNang.Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
                            ListRoles.Add(chucNang1);
                            ListChucNang.AddRange(chucNang1.Role);
                        }
                        if (dsChucNangQLKH != null && dsChucNangQLKH.Count > 0)
                        {
                            var chucNang2 = new ChucNangPartialModel();
                            chucNang2.RoleName = "QLKH";
                            chucNang2.RoleID = idNhomQLKH;
                            chucNang2.Role = new List<ChucNangModel>();
                            chucNang2.Role.AddRange(dsChucNangQLKH.Where(x => ListChucNang.Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
                            ListRoles.Add(chucNang2);
                            ListChucNang.AddRange(chucNang2.Role);
                        }
                    }

                    return Ok(new
                    {
                        Status = 1,
                        User = NguoiDung,
                        ListRole = ListChucNang,
                        Roles = ListRoles
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Status = -1,
                        Message = (User.AccessToken != null && User.AccessToken != "") ? Constant.NOT_ACCOUNT_SSO : Constant.NOT_ACCOUNT
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message, "Đăng nhập hệ thống");
                throw;
            }


        }

        [Route("EncryptPassword")]
        [HttpGet]
        public IActionResult EncryptPassword(string password)
        {
            try
            {
                return Ok(new
                {
                    Status = 1,
                    Data = Cryptor.Encrypt(password, true)
                }); ;
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = -1,
                    Data = "",
                    Mess = ex
                });
                throw;
            }
        }

        [Route("DecryptPassword")]
        [HttpGet]
        public IActionResult DecryptPassword(string password)
        {
            try
            {
                return Ok(new
                {
                    Status = 1,
                    //Data = Cryptor.DecryptPasswordUser(password)
                    Data = Cryptor.Decrypt(password, true)
                }); ;
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = -1,
                    Data = "",
                    Mess = ex
                });
                throw;
            }
        }

        [Route("PasswordToBase64")]
        [HttpGet]
        public IActionResult PasswordToBase64(string password)
        {
            try
            {
                return Ok(new
                {
                    Status = 1,
                    Data = Cryptor.PasswordToBase64(password)
                }); ;
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = -1,
                    Data = ""
                });
                throw;
            }
        }

    }
}