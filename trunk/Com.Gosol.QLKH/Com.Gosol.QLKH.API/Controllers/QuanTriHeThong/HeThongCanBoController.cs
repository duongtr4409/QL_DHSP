using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Diagnostics;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.API.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Com.Gosol.QLKH.API.Config;
using Microsoft.Extensions.Caching.Memory;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.BUS.QLKH;

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    [Route("api/v1/HeThongCanBo")]
    [ApiController]
    public class HeThongCanBoController : BaseApiController
    {
        private IHeThongCanBoBUS _HeThongCanBoBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private ISystemConfigBUS _SystemConfigBUS;
        private IHostingEnvironment _host;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IMemoryCache _cache;
        public HeThongCanBoController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, IHeThongCanBoBUS HeThongCanBoBUS, IFileDinhKemBUS FileDinhKemBUS, ISystemConfigBUS SystemConfigBUS, IHostingEnvironment HostingEnvironment, ILogHelper _LogHelper, ILogger<HeThongCanBoController> logger) : base(_LogHelper, logger)
        {

            this._HeThongCanBoBUS = HeThongCanBoBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            this._SystemConfigBUS = SystemConfigBUS;
            this._host = HostingEnvironment;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            _cache = memoryCache;
        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.ht_tai_khoan, AccessLevel.Create)]
        [Route("Insert")]
        public async Task<IActionResult> InsertAsync(IList<IFormFile> files, [FromForm] string HeThongCanBo)
        {
            try
            {
                var HeThongCanBoModel = JsonConvert.DeserializeObject<HeThongCanBoModel>(HeThongCanBo);
                HeThongCanBoModel.CoQuanID = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                //return CreateActionResult(ConstantLogMessage.HT_CanBo_ThemCanBo, EnumLogType.Insert, async () =>
                //{
                string Message = null;
                int val = 0;
                int CanBoID = 0;
                val = _HeThongCanBoBUS.Insert(HeThongCanBoModel, ref CanBoID, ref Message);
                if (val > 0)
                {
                    if (files != null && files.Count > 0)
                    {
                        var clsCommon = new Commons();
                        var CanBoIDLogin = 0;
                        try
                        {
                            CanBoIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                        }
                        catch (Exception) { }

                        foreach (IFormFile source in files)
                        {
                            FileDinhKemModel file = new FileDinhKemModel();
                            file.NghiepVuID = CanBoID;
                            file.CoQuanID = HeThongCanBoModel.CoQuanID;
                            file.LoaiFile = EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode();
                            file.FolderPath = nameof(EnumLoaiFileDinhKem.AnhDaiDien);
                            file.NguoiTaoID = CanBoIDLogin;
                            await clsCommon.InsertFileAsync(source, file, _host, _FileDinhKemBUS);
                            //var response = clsCommon.InsertFileAsync(source, file, _host, _FileDinhKemBUS);
                        }
                    }

                    base.Status = 1;
                }
                base.Message = Message;
                base.Data = val;
                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }

        }
        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.ht_tai_khoan, AccessLevel.Edit)]
        [Route("Update")]
        public async Task<IActionResult> Update(IList<IFormFile> files, [FromForm] string HeThongCanBo)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_CanBo_SuaCanBo, EnumLogType.Update, () =>
                //{
                var HeThongCanBoModel = JsonConvert.DeserializeObject<HeThongCanBoModel>(HeThongCanBo);
                string Message = null;
                int val = 0;
                val = _HeThongCanBoBUS.Update(HeThongCanBoModel, ref Message);
                if (val > 0 && files != null && files.Count > 0)
                {
                    //Xóa file cũ
                    _FileDinhKemBUS.Delete(EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode(), HeThongCanBoModel.CanBoID);
                    //Thêm file mới
                    var clsCommon = new Commons();
                    var CanBoID = 0;
                    try
                    {
                        CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    }
                    catch (Exception) { }
                    foreach (IFormFile source in files)
                    {
                        FileDinhKemModel file = new FileDinhKemModel();
                        file.NghiepVuID = HeThongCanBoModel.CanBoID;
                        file.CoQuanID = HeThongCanBoModel.CoQuanID;
                        file.LoaiFile = EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode();
                        file.FolderPath = nameof(EnumLoaiFileDinhKem.AnhDaiDien);
                        file.NguoiTaoID = CanBoID;
                        await clsCommon.InsertFileAsync(source, file, _host, _FileDinhKemBUS);
                    }
                }
                base.Message = Message;
                base.Status = val > 0 ? 1 : 0;
                base.Data = Data;
                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }

        }
        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.ht_tai_khoan, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_XoaCanBo, EnumLogType.Delete, () =>
                 {

                     var Result = _HeThongCanBoBUS.Delete(p.ListID);
                     if (Result.Count > 0)
                     {
                         base.Message = "Lỗi!";
                         base.Data = Result;
                         base.Status = 0;
                         return base.GetActionResult();
                     }
                     else
                     {
                         base.Message = "Xóa thành công!";
                         base.Data = Result;
                         base.Status = 1;
                         return base.GetActionResult();
                     }
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
        //[CustomAuthAttribute(ChucNangEnum.ht_tai_khoan, AccessLevel.Read)]
        [Route("GetByID")]
        public IActionResult GetCanBoByID([FromQuery] int CanBoID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_GetByID, EnumLogType.GetByID, () =>
                 {
                     var CoQuanID = 0;
                     try
                     {
                         CoQuanID = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG	"), 999999999);
                     }
                     catch (Exception) { }
                     HeThongCanBoModel Data;
                     Data = _HeThongCanBoBUS.GetCanBoByID(CanBoID, CoQuanID);
                     if (Data != null && Data.CanBoID > 0)
                     {
                         var clsCommon = new Commons();
                         string serverPath = clsCommon.GetServerPath(HttpContext);
                         Data.AnhHoSo = (Data.AnhHoSo != string.Empty && Data.AnhHoSo != "") ? (serverPath + Data.AnhHoSo) : string.Empty;
                     }
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
        //[CustomAuthAttribute(ChucNangEnum.ht_tai_khoan, AccessLevel.Read)]
        [Route("GetListPaging")]
        public IActionResult GetPagingBySearch([FromQuery] BasePagingParams p, int? CoQuanID, int? TrangThaiID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_GetListPaging, EnumLogType.Insert, () =>
                 {
                     List<HeThongCanBoModel> Data = new List<HeThongCanBoModel>();
                     int TotalRow = 0;
                     // ds cán bộ từ core
                     var listNguoiDungInCore = rsApiInCore.core_getusers(CoQuanID ?? 0);
                     var listCanBoInCore = rsApiInCore.core_DSCanBoTheoDonVi(CoQuanID ?? 0);
                     var listCoQuanInCore = rsApiInCore.core_DSDonViTrucThuoc(0);
                     var listChucVuInCore = rsApiInCore.core_Positions();
                     var listInDB = _HeThongCanBoBUS.GetAllInCoQuanID(CoQuanID).OrderByDescending(x => x.CanBoID).ToList();
                     this.LogError("lish In DB log " + listInDB.ToString());
                     if (listInDB.Count > 0)
                     {
                         var clsCommon = new Commons();
                         string serverPath = clsCommon.GetServerPath(HttpContext);
                         foreach (var item in listInDB)
                         {
                             if (item.AnhHoSo != null && item.AnhHoSo.Length > 0)
                             {
                                 item.AnhHoSo = serverPath + item.AnhHoSo;
                             }
                         }
                     }
                     //listCanBoInCore.ForEach(x => listInDB.Add(new HeThongCanBoModel()
                     //{
                     //    NguoiDungID = x.Id,
                     //    TenNguoiDung = listNguoiDungInCore.FirstOrDefault(i => i.StaffId == x.Id).Username,
                     //    CoQuanID = x.DepartmentId,
                     //    TenCanBo = x.Name,
                     //    CanBoID = x.Id,
                     //    Email = x.Email,
                     //    LaCanBoTrongTruong = true,
                     //    TrangThaiID = x.IsMoved ? 1 : x.IsProbation ? 2 : x.IsRetired ? 3 : 0, // 0-đang làm việc 1-Chuyển công tác 2-Thử việc 3-Nghỉ hưu
                     //    TenDonViCongTac = listCoQuanInCore.FirstOrDefault(i => i.Id == x.DepartmentId).Name,
                     //}));
                     foreach (var item in listCanBoInCore)
                     {
                         var crNguoiDung = listNguoiDungInCore.FirstOrDefault(i => i.StaffId == item.Id);
                         var crCoQuan = listCoQuanInCore.FirstOrDefault(i => i.Id == item.DepartmentId);

                         var it = new HeThongCanBoModel();
                         it.NguoiDungID = item.Id;
                         it.TenNguoiDung = (crNguoiDung != null && crNguoiDung.Id > 0) ? crNguoiDung.Username : "";
                         it.CoQuanID = item.DepartmentId;
                         it.TenCanBo = item.Name;
                         it.CanBoID = item.Id;
                         it.Email = item.Email;
                         it.LaCanBoTrongTruong = true;
                         it.TrangThaiID = item.IsMoved ? 1 : item.IsProbation ? 2 : item.IsRetired ? 3 : 0; // 0-đang làm việc 1-Chuyển công tác 2-Thử việc 3-Nghỉ hưu
                         it.TenDonViCongTac = (crCoQuan != null && crCoQuan.Id > 0) ? crCoQuan.Name : "";
                         if (item.PositionIds != null && item.PositionIds.Count > 0)
                         {
                             foreach (var i in item.PositionIds)
                             {
                                 var crChucVu = listChucVuInCore.FirstOrDefault(m => m.Id == i);
                                 if (crChucVu != null && crChucVu.Id > 0)
                                 {
                                     if (it.TenChucVu != null && it.TenChucVu != string.Empty && it.TenChucVu.Length > 0)
                                         it.TenChucVu = it.TenChucVu + ", " + crChucVu.Name;
                                     else
                                         it.TenChucVu = crChucVu.Name;
                                 }
                             }
                         }
                         listInDB.Add(it);
                     }
                     var listFilter = listInDB;
                     if (p.Keyword != null && p.Keyword.Length > 0)
                     {
                         listFilter = new List<HeThongCanBoModel>();
                         listFilter = listInDB.Where(x => x.TenCanBo.ToLower().Contains(p.Keyword.ToLower()) || x.TenNguoiDung.ToLower().Contains(p.Keyword.ToLower())).ToList();
                     }
                     if (TrangThaiID != null)
                     {
                         listFilter = listFilter.Where(x => x.TrangThaiID == TrangThaiID).ToList();
                     }
                     TotalRow = listFilter.Count();
                     Data = listFilter.OrderBy(x => x.LaCanBoTrongTruong).ThenByDescending(x => x.NguoiDungID).Skip(p.Offset).Take(p.Limit).ToList();
                     if (Data.Count == 0)
                     {
                         base.Message = ConstantLogMessage.API_NoData;
                         base.Status = 1;
                         base.TotalRow = 0;
                         base.Data = Data;
                         return base.GetActionResult();
                     }
                     base.Status = TotalRow > 0 ? 1 : 0;
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
        //[HttpGet]
        //[Route("FilterByName")]
        //public IActionResult FilterByName(string TenCanBo, int IsStatus, int CoQuanID)
        //{
        //    return CreateActionResult(ConstantLogMessage.HT_CanBo_FilterByName, () =>
        //    {
        //        IList<HeThongCanBoModel> Data;
        //        Data = _HeThongCanBoBUS.FilterByName(TenCanBo, IsStatus, CoQuanID);
        //        int totalRow = Data.Count();
        //        base.Status = totalRow > 0 ? 1 : 0;
        //        base.Data = Data;
        //        base.TotalRow = totalRow;
        //        return base.GetActionResult();
        //    });
        //}
        [HttpPost]
        // [CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.Read)]
        [Route("ReadExcelFile")]
        public async Task<IActionResult> ReadExcelFile([FromBody] Files file)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_ImportFile, EnumLogType.Other, () =>
                {
                    string SavePath = _host.ContentRootPath + "\\Upload\\" + "CanBo.xlsx";
                    if (System.IO.File.Exists(SavePath))
                    {
                        System.IO.File.Delete(SavePath);
                    }
                    using (FileStream stream = System.IO.File.Create(SavePath))
                    {
                        byte[] byteArray = Convert.FromBase64String(file.files);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }

                    var Result = _HeThongCanBoBUS.ReadExcelFile(SavePath, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0));
                    //var ResultNew = Result.Where(x => x.NguyenNhan.Where(y=>y.Contains("Files không có dữ liệu!")).Select(x=>x)).ToList();
                    //if (ResultNew.Count > 0)
                    //{
                    //    base.Message = "Files không có dữ liệu!";
                    //    base.Status = 0;
                    //    return base.GetActionResult();
                    //}
                    if (Result.Where(x => x.NguyenNhan.Count > 0).ToList().Count <= 0)
                    {
                        base.Message = "Import thành công!";
                        base.Status = 1;
                        return base.GetActionResult();
                    }

                    else
                    {
                        foreach (var item in Result)
                        {
                            var FileNo = item.NguyenNhan.Where(x => x.ToString().Trim() == "Files không có dữ liệu!").ToList();
                            if (FileNo.Count > 0)
                            {
                                base.Message = "Files không có dữ liệu!";
                                base.Status = 0;
                                return base.GetActionResult();
                            }
                        }
                        base.Message = "Import không thành công!" + "<br>" + " Danh sách lỗi :" + "<br>";
                        base.Status = 0;
                        base.Data = Result;
                        return base.GetActionResult();
                    }
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.Read)]
        [Route("DowloadExel")]
        public async Task<IActionResult> DowloadExel()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_ExportFile, EnumLogType.Other, () =>
                 {

                     var host = _host.ContentRootPath;
                     var expath = host + "\\Upload\\CanBo_Template.xlsm";
                     _HeThongCanBoBUS.ImportToExel(expath, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0));
                     var memory = new MemoryStream();
                     Byte[] bytes = System.IO.File.ReadAllBytes(expath);
                     String file = Convert.ToBase64String(bytes);
                     file = string.Concat("data:application/vnd.ms-excel;base64,", file);
                     //httpResponseMessage.StatusCode = HttpStatusCode.OK;
                     base.Data = file;
                     base.Status = 1;
                     return base.GetActionResult();

                 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.FullAccess)]
        [Route("GetThanNhanByCanBoID")]
        public IActionResult GetThanNhanByCanBoID() // lấy thân nhân của cán bộ đang đăng nhập
        {
            try
            {
                return CreateActionResult("Lấy thân nhân theo cán bộ", EnumLogType.Insert, () =>
                {
                    List<HeThongCanBoShortModel> list = new List<HeThongCanBoShortModel>();
                    list = _HeThongCanBoBUS.GetThanNhanByCanBoID(Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "CanBoID").Value, 0));
                    base.Status = 1;
                    Data = list;
                    base.Message = "Danh sách thân nhân";
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
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.FullAccess)]
        [Route("GetAllCanBoByCoQuanID")]
        public IActionResult GetAllCanBoByCoQuanID(int CoQuanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_GetListPaging, EnumLogType.Other, () =>
                {
                    var Result = _HeThongCanBoBUS.GetAllCanBoByCoQuanID(CoQuanID, Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0));
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.FullAccess)]
        [Route("GetAllCanBoTrongCoQuan")]
        public IActionResult GetAllCanBoTrongCoQuan()
        {
            try
            {
                return CreateActionResult("Lấy tất cả cán bộ trong cơ quan", EnumLogType.Other, () =>
                {
                    var Result = _HeThongCanBoBUS.GetAllByCoQuanID(Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0));
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.Read)]
        [Route("GenerationMaCanBo")]
        public IActionResult GenerationMaCanBo([FromQuery] int CoQuanID)
        {
            try
            {
                return CreateActionResult("Tạo mã bởi cơ quan", EnumLogType.GetByID, () =>
                {
                    var Data = _HeThongCanBoBUS.GenerationMaCanBo(CoQuanID);
                    if (string.IsNullOrEmpty(Data))
                    {
                        base.Status = 0;
                        base.Data = Data;
                        return base.GetActionResult();

                    }
                    base.Status = 1;
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
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.FullAccess)]
        [Route("GetAllInCoQuanCha")]
        public IActionResult GetAllInCoQuanCha([FromQuery] int? CoQuanID)
        {
            try
            {
                return CreateActionResult("Lấy tất cả cán bộ trong cơ quan", EnumLogType.Other, () =>
                {
                    var Result = _HeThongCanBoBUS.GetAllInCoQuanCha(CoQuanID.Value);
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.FullAccess)]
        [Route("HeThongCanBo_GetThongTinCoQuan")]
        public IActionResult HeThongCanBo_GetThongTinCoQuan()
        {
            try
            {
                return CreateActionResult("Lấy thông tin cơ quan của cán bộ", EnumLogType.Other, () =>
                {
                    var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CanBoID")).Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("NguoiDungID")).Value, 0);
                    var Result = _HeThongCanBoBUS.HeThongCanBo_GetThongTinCoQuan(CanBoID, NguoiDungID);
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("HeThongCanBo_GetAllCanBo")]
        public IActionResult HeThongCanBo_GetAllCanBo()
        {
            try
            {
                return CreateActionResult("Lấy danh sách cán bộ", EnumLogType.Other, () =>
                {
                    var Result = _HeThongCanBoBUS.HeThongCanBo_GetAllCanBo();
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.FullAccess)]
        [Route("GetDanhSachLeTan")]
        public IActionResult GetDanhSachLeTan()
        {
            try
            {
                return CreateActionResult("Lấy danh sách lễ tân", EnumLogType.GetList, () =>
                {
                    var Result = new List<HeThongCanBoModel>();
                    if (Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CanBoID")).Value, 0) == 1)
                    {
                        Result = _HeThongCanBoBUS.GetDanhSachLeTan();
                    }
                    else
                    {
                        var coquanid = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0);
                        Result = _HeThongCanBoBUS.GetDanhSachLeTan_TrongCoQuanSuDungPhanMem(coquanid);
                    }

                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// lấy toàn bộ danh sách cán bộ thuộc hệ thống của cơ quan sử dụng phần mềm theo cơ quan đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_CanBo, AccessLevel.FullAccess)]
        [Route("DanhSachCanBo_TrongCoQuanSuDungPhanMem")]
        public IActionResult DanhSachCanBo_TrongCoQuanSuDungPhanMem()
        {
            try
            {
                return CreateActionResult("Lấy tất cả cán bộ trong cơ quan", EnumLogType.Other, () =>
                {
                    var Result = new List<HeThongCanBoModel>();
                    if (Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CanBoID")).Value, 0) == 1)
                    {
                        Result = _HeThongCanBoBUS.HeThongCanBo_GetAllCanBo();
                    }
                    else
                        Result = _HeThongCanBoBUS.DanhSachCanBo_TrongCoQuanSuDungPhanMem(Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0));
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }
        }
    }
}
