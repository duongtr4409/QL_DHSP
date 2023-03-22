using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static Com.Gosol.QLKH.Models.QuanTriHeThong.HeThongNguoiDungModelPartial;
//using LogHelper = Com.Gosol.QLKH.API.Formats.LogHelper;

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    [Route("api/v1/HeThongNguoiDung")]
    [ApiController]
    public class HeThongNguoidungController : BaseApiController
    {
        private IHeThongNguoidungBUS _HeThongNguoidungBUS;
        private ILogHelper _ILogHelper;
        private IPhanQuyenBUS _PhanQuyenBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private ISystemConfigBUS _SystemConfigBUS;

        public HeThongNguoidungController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, IHeThongNguoidungBUS HeThongNguoidungBUS, ISystemConfigBUS SystemConfigBUS, ILogHelper _logHelper, IPhanQuyenBUS PhanQuyenBUS, ILogger<HeThongNguoidungController> logger) : base(_logHelper, logger)
        {
            this._HeThongNguoidungBUS = HeThongNguoidungBUS;
            _PhanQuyenBUS = PhanQuyenBUS;
            this._AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._SystemConfigBUS = SystemConfigBUS;
        }
        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(HeThongNguoiDungModel HeThongNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_ThemNguoidung, EnumLogType.Insert, () =>
                {
                    string Message = null;
                    int val = 0;
                    val = _HeThongNguoidungBUS.Insert(HeThongNguoiDungModel, ref Message, ref val);
                    base.Message = Message;
                    base.Status = val > 0 ? 1 : 0;
                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(HeThongNguoiDungModel HeThongNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_SuaNguoidung, EnumLogType.Update, () =>
                {
                    string Message = null;
                    int val = 0;
                    val = _HeThongNguoidungBUS.Update(HeThongNguoiDungModel, ref Message);
                    base.Message = Message;
                    base.Status = val > 0 ? 1 : 0;
                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_XoaNguoidung, EnumLogType.Delete, () =>
                 {
                     int Status = 0;
                     var Result = _HeThongNguoidungBUS.Delete(p.ListID, ref Status);
                     //if(Result.Count <= 0)
                     //{
                     //    base.Status = 1;
                     //    base.Message = "Xóa thành công!";
                     //    return base.GetActionResult();
                     //}
                     //else
                     //{
                     base.Status = Status;
                     base.Data = Result;
                     return base.GetActionResult();
                     //}

                 });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Read)]
        [Route("GetByID")]
        public IActionResult GetByID(int NguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_GetByID, EnumLogType.GetByID, () =>
                 {
                     HeThongNguoiDungModel Data;
                     Data = _HeThongNguoidungBUS.GetByID(NguoiDungID);
                     base.Status = Data.CanBoID > 0 ? 1 : 0;
                     base.Data = Data;
                     return base.GetActionResult();
                 });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Read)]
        [Route("GetListPaging")]
        public IActionResult GetPagingBySearch1([FromQuery] BasePagingParams p, int? CoQuanID, int? TrangThai)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_GetListPaging, EnumLogType.GetList, () =>
                 {
                     CoQuanID = CoQuanID ?? 0;
                     int TotalRow = 0;
                     // ds người dùng từ core
                     var listInCore = rsApiInCore.core_getusers(CoQuanID.Value);
                     var listCanBoInCore = rsApiInCore.core_DSCanBoTheoDonVi(CoQuanID.Value);
                     var listInDB = _HeThongNguoidungBUS.GetAll();
                     listInCore.ForEach(x => listInDB.Add(new HeThongNguoiDungModel()
                     {
                         NguoiDungID = x.Id,
                         TenNguoiDung = x.Name,
                         CoQuanID = x.DepartmentId,
                         TenCanBo = listCanBoInCore.FirstOrDefault(i => i.Id == x.StaffId).Name,

                     }));

                     int totalRow = listInDB.Count();
                     if (totalRow == 0)
                     {
                         base.Message = ConstantLogMessage.API_NoData;
                         base.Status = 1;
                         return base.GetActionResult();
                     }

                     base.Status = TotalRow > 0 ? 1 : 0;
                     base.Data = listInDB;
                     base.TotalRow = TotalRow;
                     return base.GetActionResult();
                 });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }


        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Read)]
        [Route("ResetPassword_old")]
        public IActionResult ResetPassword([FromQuery] int NguoiDungID)
        {
            try
            {
                return CreateActionResult("Reset lại mật khẩu", EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.ResetPassword(NguoiDungID);
                    base.Message = Result.FirstOrDefault().Value.ToString();
                    base.Status = Utils.ConvertToInt32(Result.FirstOrDefault().Key, 0);
                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        /// <summary>
        /// Reset cả tải khoản trong trường, (gọi tới core)
        /// </summary>
        /// <param name="NguoiDungID"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.ht_tai_khoan, AccessLevel.Edit)]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([FromQuery] int NguoiDungID, int? CoQuanID)
        {
            try
            {
                return CreateActionResult("Reset lại mật khẩu", EnumLogType.Other, () =>
                {
                    var idCoQuanNgoaiTruong = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 0);
                    if (CoQuanID != null && CoQuanID != idCoQuanNgoaiTruong)
                    {
                        var newPassword = Utils.ConvertToString(_SystemConfigBUS.GetByKey("MATKHAU_MACDINH").ConfigValue, "123456");
                        var query = rsApiInCore.core_changepassword(NguoiDungID, newPassword);
                        if (query)
                        {
                            base.Message = "Reset mật khẩu thành công!";
                            base.Status = 1;
                            base.Data = Data;
                            return base.GetActionResult();
                        }
                        else
                        {
                            base.Message = "Reset mật khẩu không thành công!";
                            base.Status = 0;
                            base.Data = Data;
                            return base.GetActionResult();
                        }
                    }
                    var Result = _HeThongNguoidungBUS.ResetPassword(NguoiDungID);
                    base.Message = Result.FirstOrDefault().Value.ToString();
                    base.Status = Utils.ConvertToInt32(Result.FirstOrDefault().Key, 0);
                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [Route("GetByIDForPhanQuyen")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.FullAccess)]
        [HttpGet]
        public IActionResult GetByIDForPhanQuyen(int? NguoiDungID)
        {
            try
            {

                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_GetByID, EnumLogType.GetList, () =>
                {
                    if (NguoiDungID == null)
                    {
                        return Ok(new
                        {
                            Status = -1,
                            Message = "Param NguoiDungID is NULL",
                        });
                    }
                    NguoiDungModel NguoiDung = new NguoiDungModel();

                    var coQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var coQuanNgoaiTruong = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                    if (coQuanID != coQuanNgoaiTruong)
                    {
                        NguoiDung.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                        NguoiDung.CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                        NguoiDung.TenNguoiDung = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "TenNguoiDung").Value, string.Empty);
                        NguoiDung.TenCanBo = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "TenCanBo").Value, string.Empty);
                        NguoiDung.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    }
                    else

                        NguoiDung = _HeThongNguoidungBUS.GetByIDForPhanQuyen(NguoiDungID.Value);

                    if (NguoiDung != null && NguoiDung.NguoiDungID > 0)
                    {
                        var ListChucNang = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDungID.Value);
                        NguoiDung.expires_at = Utils.ConvertToDateTime(User.Claims.FirstOrDefault(c => c.Type == "expires_at").Value, DateTime.Now.Date);
                        NguoiDung.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);

                        var ListRoles = new List<ChucNangPartialModel>();
                        if (_PhanQuyenBUS.CheckAdmin(NguoiDung.NguoiDungID))
                        {
                            var roleAdmin = new ChucNangPartialModel();
                            roleAdmin.RoleName = "Admin";
                            roleAdmin.Role = new List<ChucNangModel>();
                            roleAdmin.Role.AddRange(ListChucNang.Where(x => x.ChucNangID >= 200 && x.ChucNangID < 300).ToList());
                            roleAdmin.RoleID = 0;
                            ListRoles.Add(roleAdmin);
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
                                chucNang1.RoleName = "QLDLDV";
                                chucNang1.RoleID = idNhomTruongKhoa;
                                chucNang1.Role = new List<ChucNangModel>();
                                chucNang1.Role.AddRange(dsChucNangTruongKhoa.Where(x => ListChucNang.Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
                                ListRoles.Add(chucNang1);
                            }
                            if (dsChucNangQLKH != null && dsChucNangQLKH.Count > 0)
                            {
                                var chucNang2 = new ChucNangPartialModel();
                                chucNang2.RoleName = "QLKH";
                                chucNang2.RoleID = idNhomQLKH;
                                chucNang2.Role = new List<ChucNangModel>();
                                chucNang2.Role.AddRange(dsChucNangQLKH.Where(x => ListChucNang.Select(y => y.MaChucNang).ToList().Contains(x.MaChucNang)).ToList());
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
                            //Message = Constant.NOT_ACCOUNT,
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw ex;
            }
        }

        [Route("SendMail")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.FullAccess)]
        [HttpPost]
        public IActionResult SendMail([FromBody] HeThongNguoiDungModelPartial p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.API_SendMail, EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.SendMail(p.TenNguoiDung, p.Url);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }
        [Route("UpdateNguoiDung")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.FullAccess)]
        [HttpPost]
        public IActionResult UpdateNguoiDung([FromBody] QuenMatKhauModelPar p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.API_SendMail, EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.UpdateNguoiDung(p.TenDangNhap, p.MatKhauMoi);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }



        }
        [Route("CheckMaMail")]
        [HttpGet]
        public IActionResult CheckMaMail([FromQuery] string Token)
        {
            try
            {
                return CreateActionResult("Check mã mail", EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.CheckMaMail(Token);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    base.Data = Result.Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }



        }
        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Read)]
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody] DoiMatKhauModel p)
        {
            try
            {
                //var expires_at = Utils.ConvertToDateTime(User.Claims.FirstOrDefault(c => c.Type == "expires_at").Value, DateTime.Now.Date);
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                return CreateActionResult("Đổi mật khẩu", EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.ChangePassword(NguoiDungID, p.OldPassword, p.NewPassword);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_NguoiDung, AccessLevel.Read)]
        [Route("HeThong_NguoiDung_GetListBy_NhomNguoiDungID")]
        public IActionResult HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult("Lấy danh sách người dùng theo nhóm người dùng", EnumLogType.GetList, () =>
                {
                    IList<HeThongNguoiDungModel> Data;
                    int TotalRow = 0;
                    //Data = _HeThongNguoidungBUS.HeThong_NguoiDung_GetListBy_NhomNguoiDungID(NhomNguoiDungID);
                    Data = _PhanQuyenBUS.HeThong_NguoiDung_GetListBy_NhomNguoiDungID(NhomNguoiDungID, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0));
                    int totalRow = Data.Count();
                    if (totalRow == 0)
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                        base.Status = 1;
                        base.GetActionResult();
                    }
                    base.Status = totalRow >= 0 ? 1 : 0;
                    base.Data = Data;
                    base.TotalRow = TotalRow;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
    }
}