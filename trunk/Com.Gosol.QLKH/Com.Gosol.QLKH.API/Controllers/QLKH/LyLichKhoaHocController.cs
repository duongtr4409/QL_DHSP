using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace Com.Gosol.QLKH.API.Controllers.QLKH
{
    [Route("api/v1/LyLichKhoaHoc")]
    [ApiController]
    public class LyLichKhoaHocController : BaseApiController
    {
        private ILyLichKhoaHocBUS _LyLichKhoaHocBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IPhanQuyenBUS _PhanQuyenBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;
        private ISystemConfigBUS _SystemConfigBUS;
        private IHeThongCanBoBUS _HeThongCanBoBUS;
        public LyLichKhoaHocController(IHeThongCanBoBUS HeThongCanBoBUS, ISystemConfigBUS SystemConfigBUS, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, ILyLichKhoaHocBUS LyLichKhoaHocBUS, IFileDinhKemBUS FileDinhKemBUS, IPhanQuyenBUS PhanQuyenBUS, ILogHelper _LogHelper, ILogger<DeTaiController> logger) : base(_LogHelper, logger)
        {
            this._LyLichKhoaHocBUS = LyLichKhoaHocBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            this._PhanQuyenBUS = PhanQuyenBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
            this._SystemConfigBUS = SystemConfigBUS;
            this._HeThongCanBoBUS = HeThongCanBoBUS;
        }

        [HttpGet]
        [Route("NhaKhoaHoc_DanhSach")]
        //[CustomAuthAttribute(ChucNangEnum.ql_nha_khoa_hoc, AccessLevel.Read)]
        public IActionResult NhaKhoaHoc_DanhSach([FromQuery] BasePagingParams p, int? QuyenQuanLy, string Alphabet)
        {
            try
            {
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                return CreateActionResult("Lấy danh sách nhà khoa học", EnumLogType.GetList, () =>
                       {
                           var data = new List<NhaKhoaHocModel>();
                           int TotalRow = 0;
                           // ngoài trường
                           if (QuyenQuanLy == 0)
                           {
                               data = _LyLichKhoaHocBUS.GetPagingBySearch(p, ref TotalRow, QuyenQuanLy ?? 0, Alphabet ?? "");
                               data.ForEach(x => x.AnhHoSo = (x.AnhHoSo != "" && x.AnhHoSo != null && x.AnhHoSo != string.Empty) ? (serverPath + x.AnhHoSo) : "");
                               //Phân quyền quản lý
                               //var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                               //var CanBoIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                               //var CoQuanIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                               //int quyen = _PhanQuyenBUS.CheckQuyen(NguoiDungID);
                               List<CoreDataModel> listCanBoInCore = new List<CoreDataModel>();

                               var listCoQuanInCore = rsApiInCore.core_DSDonViTrucThuoc(0);
                               var result = rsApiInCore.core_DSCanBoTheoDonVi(0);
                               foreach (var item in result)
                               {
                                   listCanBoInCore.Add(item);
                               }
                               //if (quyen == EnumQuyenQuanLy.QuanLy.GetHashCode())
                               //{
                               //    foreach (var item in result)
                               //    {
                               //        listCanBoInCore.Add(item);
                               //    }
                               //}
                               //else if (quyen == EnumQuyenQuanLy.TruongKhoa.GetHashCode() && CoQuanIDLogin != 999999999)
                               //{
                               //    //int? CoQuanID = 0;
                               //    //foreach (var item in result)
                               //    //{
                               //    //    if (item.Id == CanBoIDLogin)
                               //    //    {
                               //    //        CoQuanID = item.DepartmentId;
                               //    //        break;
                               //    //    }
                               //    //}
                               //    foreach (var item in result)
                               //    {
                               //       /* if (item.DepartmentId == CoQuanID)*/
                               //        listCanBoInCore.Add(item);
                               //    }
                               //}

                               listCanBoInCore =
                                listCanBoInCore.Where(x =>
                                       (x.Name.ToLower().Contains(((p.Keyword != null && p.Keyword != "" && p.Keyword.Length > 0) ? p.Keyword.ToLower() : x.Name.ToLower())))
                                       && (Alphabet == null || Alphabet == "" || Alphabet == string.Empty || Alphabet.ToLower() == x.FName.ToLower())
                                       ).ToList();
                               var resul = new List<CanBoCoreModel>();
                               TotalRow = listCanBoInCore.Count();
                               listCanBoInCore = listCanBoInCore.OrderBy(x => x.Name).Skip(p.Offset).Take(p.Limit).ToList();
                               foreach (var item in listCanBoInCore)
                               {
                                   var it = new NhaKhoaHocModel();
                                   it.CanBoID = item.Id;
                                   it.TenCanBo = item.Name;
                                   it.MaCB = item.Code;
                                   it.NgaySinh = item.Birthday;
                                   it.ChucDanhHanhChinh = item.PositionIds.ToList();
                                   it.ChucDanhKhoaHoc = new List<int>() { item.DegreeId };
                                   it.CoQuanID = item.DepartmentId;
                                   it.GioiTinhStr = item.Gender;
                                   it.Email = item.Email;
                                   it.NgaySinh = item.Birthday;
                                   it.FName = item.FName;
                                   var cqCuaCanBo = listCoQuanInCore.FirstOrDefault(x => x.Id == item.DepartmentId);
                                   if (cqCuaCanBo != null) it.CoQuanCongTac = cqCuaCanBo.Name;
                                   //lấy ảnh hồ sơ
                                   var FileDinhKem = _FileDinhKemBUS.GetByLoaiFileAndNghiepVuIDAndCoQuanID(EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode(), it.CanBoID, it.CoQuanID);
                                   if (FileDinhKem != null && FileDinhKem.Count > 0)
                                   {
                                       foreach (var file in FileDinhKem)
                                       {
                                           file.FileUrl = serverPath + file.FileUrl;
                                           it.AnhHoSo = file.FileUrl;
                                       }
                                   }
                                   data.Add(it);
                               }
                           }
                           else if (QuyenQuanLy == 2)
                           {
                               data = _LyLichKhoaHocBUS.GetPagingBySearch(p, ref TotalRow, QuyenQuanLy ?? 0, Alphabet ?? "");
                               data.ForEach(x => x.AnhHoSo = (x.AnhHoSo != "" && x.AnhHoSo != null && x.AnhHoSo != string.Empty) ? (serverPath + x.AnhHoSo) : "");
                           }
                           // trong trường
                           else
                           {
                               //Phân quyền quản lý
                               var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                               var CanBoIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                               var CoQuanIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                               int quyen = _PhanQuyenBUS.CheckQuyen(NguoiDungID);
                               List<CoreDataModel> listCanBoInCore = new List<CoreDataModel>();

                               var listCoQuanInCore = rsApiInCore.core_DSDonViTrucThuoc(0);
                               var result = rsApiInCore.core_DSCanBoTheoDonVi(0);

                               if (quyen == EnumQuyenQuanLy.QuanLy.GetHashCode())
                               {
                                   foreach (var item in result)
                                   {
                                       listCanBoInCore.Add(item);
                                   }
                               }
                               else if (quyen == EnumQuyenQuanLy.TruongKhoa.GetHashCode() && CoQuanIDLogin != 999999999)
                               {
                                   int? CoQuanID = 0;
                                   foreach (var item in result)
                                   {
                                       if (item.Id == CanBoIDLogin)
                                       {
                                           CoQuanID = item.DepartmentId;
                                           break;
                                       }
                                   }
                                   foreach (var item in result)
                                   {
                                       if (item.DepartmentId == CoQuanID) listCanBoInCore.Add(item);
                                   }
                               }

                               listCanBoInCore =
                                listCanBoInCore.Where(x =>
                                       (x.Name.ToLower().Contains(((p.Keyword != null && p.Keyword != "" && p.Keyword.Length > 0) ? p.Keyword.ToLower() : x.Name.ToLower())))
                                       && (Alphabet == null || Alphabet == "" || Alphabet == string.Empty || Alphabet.ToLower() == x.FName.ToLower())
                                       ).ToList();
                               var resul = new List<CanBoCoreModel>();
                               TotalRow = listCanBoInCore.Count();
                               listCanBoInCore = listCanBoInCore.OrderBy(x => x.FName).Skip(p.Offset).Take(p.Limit).ToList();
                               foreach (var item in listCanBoInCore)
                               {
                                   var it = new NhaKhoaHocModel();
                                   it.CanBoID = item.Id;
                                   it.TenCanBo = item.Name;
                                   it.MaCB = item.Code;
                                   it.NgaySinh = item.Birthday;
                                   it.ChucDanhHanhChinh = item.PositionIds.ToList();
                                   it.ChucDanhKhoaHoc = new List<int>() { item.DegreeId };
                                   it.CoQuanID = item.DepartmentId;
                                   it.GioiTinhStr = item.Gender;
                                   it.Email = item.Email;
                                   it.NgaySinh = item.Birthday;
                                   it.FName = item.FName;
                                   var cqCuaCanBo = listCoQuanInCore.FirstOrDefault(x => x.Id == item.DepartmentId);
                                   if (cqCuaCanBo != null) it.CoQuanCongTac = cqCuaCanBo.Name;
                                   //lấy ảnh hồ sơ
                                   var FileDinhKem = _FileDinhKemBUS.GetByLoaiFileAndNghiepVuIDAndCoQuanID(EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode(), it.CanBoID, it.CoQuanID);
                                   if (FileDinhKem != null && FileDinhKem.Count > 0)
                                   {
                                       foreach (var file in FileDinhKem)
                                       {
                                           file.FileUrl = serverPath + file.FileUrl;
                                           it.AnhHoSo = file.FileUrl;
                                       }
                                   }
                                   data.Add(it);
                               }
                           }
                           if (data != null && data.Count > 0)
                           {
                               data = data.OrderBy(x => x.FName).ToList();
                           }
                           base.Status = 1;
                           base.TotalRow = TotalRow;
                           base.Data = data;
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

        [HttpPost]
        [Route("NhaKhoaHoc_ThemMoi")]
        //[CustomAuthAttribute(ChucNangEnum.ql_nha_khoa_hoc, AccessLevel.Create)]
        public async Task<IActionResult> NhaKhoaHoc_ThemMoi(IList<IFormFile> files, IList<IFormFile> AnhDaiDien, [FromForm] string ThongTinNhaKhoaHoc)
        {
            try
            {
                var NhaKhoaHocModel = JsonConvert.DeserializeObject<NhaKhoaHocModel>(ThongTinNhaKhoaHoc);
                var Result = _LyLichKhoaHocBUS.Insert(NhaKhoaHocModel);
                int NhaKhoaHocID = Utils.ConvertToInt32(Result.Data, 0);
                int CoQuanID = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                if (NhaKhoaHocID > 0 && files != null && files.Count > 0)
                {
                    var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var clsCommon = new Commons();
                    foreach (IFormFile source in files)
                    {
                        FileDinhKemModel file = new FileDinhKemModel();
                        file.NghiepVuID = NhaKhoaHocID;
                        file.LoaiFile = EnumLoaiFileDinhKem.LyLich.GetHashCode();
                        file.FolderPath = nameof(EnumLoaiFileDinhKem.LyLich);
                        file.NguoiTaoID = CanBoID;
                        file.CoQuanID = CoQuanID;
                        //file.NoiDung = NoiDung;
                        await clsCommon.InsertFileAsync(source, file, _host, _FileDinhKemBUS);
                    }
                }
                if (NhaKhoaHocID > 0 && AnhDaiDien != null && AnhDaiDien.Count > 0)
                {
                    var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var clsCommon = new Commons();
                    foreach (IFormFile source in AnhDaiDien)
                    {
                        FileDinhKemModel file = new FileDinhKemModel();
                        file.NghiepVuID = NhaKhoaHocID;
                        file.LoaiFile = EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode();
                        file.FolderPath = nameof(EnumLoaiFileDinhKem.AnhDaiDien);
                        file.NguoiTaoID = CanBoID;
                        file.CoQuanID = CoQuanID;
                        await clsCommon.InsertFileAsync(source, file, _host, _FileDinhKemBUS);
                    }
                }

                base.Status = Result.Status;
                base.Message = Result.Message;
                base.Data = Result.Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }

        }

        [HttpPost]
        [Route("NhaKhoaHoc_Delete")]
        [CustomAuthAttribute(ChucNangEnum.ql_nha_khoa_hoc, AccessLevel.Create)]
        public IActionResult NhaKhoaHoc_Delete(NhaKhoaHocModel NhaKhoaHocModel)
        {
            try
            {
                var Result = _LyLichKhoaHocBUS.Delete(NhaKhoaHocModel);
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }

        }

        [HttpGet]
        [Route("NhaKhoaHoc_ChiTiet")]
        //[CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Read)]
        public IActionResult NhaKhoaHoc_ChiTiet(int CanBoID, int? CoQuanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var coQuanNgoaiTruong = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                    NhaKhoaHocModel Data = new NhaKhoaHocModel();
                    Data = _LyLichKhoaHocBUS.GetByID(CanBoID, serverPath, CoQuanID);
                    Data.AnhHoSo = (Data.AnhHoSo != null && Data.AnhHoSo != "" && Data.AnhHoSo != string.Empty) ? (serverPath + Data.AnhHoSo) : "";

                    // cơ quan trong trường
                    if (CoQuanID != null && CoQuanID != coQuanNgoaiTruong)
                    {
                        var canBoInfo = rsApiInCore.core_getstaff(CanBoID);
                        Data.CanBoID = canBoInfo.Id;
                        Data.TenCanBo = canBoInfo.Name;
                        Data.MaCB = canBoInfo.Code;
                        Data.NgaySinh = canBoInfo.Birthday;
                        Data.CoQuanID = canBoInfo.DepartmentId;
                        Data.PhongBanID = canBoInfo.DepartmentId;
                        Data.Email = canBoInfo.Email;
                        Data.CoQuanCongTac = "";
                        Data.GioiTinhStr = canBoInfo.Gender;
                        Data.ChucDanhKhoaHoc = new List<int>() { canBoInfo.DegreeId };
                        Data.ChucDanhHanhChinh = canBoInfo.PositionIds.ToList();
                        Data.LaCanBoTrongTruong = true;
                        var FileDinhKem = _FileDinhKemBUS.GetByLoaiFileAndNghiepVuIDAndCoQuanID(EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode(), Data.CanBoID, Data.CoQuanID);
                        if (FileDinhKem != null && FileDinhKem.Count > 0)
                        {
                            foreach (var item in FileDinhKem)
                            {
                                item.FileUrl = serverPath + item.FileUrl;
                                Data.AnhHoSo = item.FileUrl;
                            }
                        }
                        Data.Url = _LyLichKhoaHocBUS.GetUrlCanBoTrongTruong(Data.CanBoID, Data.CoQuanID);
                        Data.NguoiGioiThieu = _LyLichKhoaHocBUS.GetNguoiGioiThieu(Data.CanBoID, Data.CoQuanID);
                        Data.TrangThaiID = 0;
                        if (canBoInfo.IsMoved)
                        {
                            Data.TrangThaiID = 1;
                        }
                        if (canBoInfo.IsProbation)
                        {
                            Data.TrangThaiID = 2;
                        }
                        if (canBoInfo.IsRetired)
                        {
                            Data.TrangThaiID = 3;
                        }
                    }
                    // cơ quan ngoài trường
                    else if (Data.CanBoID == null || Data.CanBoID < 1 || Data.TenCanBo == null || Data.TenCanBo == string.Empty || Data.TenCanBo == "")
                    {
                        var canBoNgoaiTruong = _HeThongCanBoBUS.GetCanBoByID(CanBoID, 0);
                        if (canBoNgoaiTruong != null && canBoNgoaiTruong.CanBoID > 0)
                        {
                            Data.AnhHoSo = canBoNgoaiTruong.AnhHoSo;
                            Data.AnhHoSo = (Data.AnhHoSo != null && Data.AnhHoSo != "" && Data.AnhHoSo != string.Empty) ? (serverPath + Data.AnhHoSo) : "";
                            Data.CanBoID = canBoNgoaiTruong.CanBoID;
                            Data.TenCanBo = canBoNgoaiTruong.TenCanBo;
                            Data.MaCB = canBoNgoaiTruong.MaCB;
                            Data.NgaySinh = canBoNgoaiTruong.NgaySinh;
                            Data.CoQuanID = canBoNgoaiTruong.CoQuanID.Value;
                            Data.PhongBanID = canBoNgoaiTruong.PhongBanID;
                            Data.CoQuanCongTac = canBoNgoaiTruong.CoQuanCongTac;
                            Data.DiaChiCoQuan = canBoNgoaiTruong.DiaChiCoQuan;
                            Data.GioiTinh = canBoNgoaiTruong.GioiTinh;
                            Data.ChucDanhKhoaHoc = canBoNgoaiTruong.ChucDanhKhoaHoc;
                            Data.ChucDanhHanhChinh = canBoNgoaiTruong.ChucDanhHanhChinh;
                            Data.Email = canBoNgoaiTruong.Email;
                            Data.DienThoai = canBoNgoaiTruong.DienThoai;
                            Data.DienThoaiDiDong = canBoNgoaiTruong.DienThoaiDiDong;
                            Data.Fax = canBoNgoaiTruong.Fax;
                            Data.Url = canBoNgoaiTruong.Url;
                            Data.TrangThaiID = canBoNgoaiTruong.TrangThaiID;
                            Data.LaChuyenGia = canBoNgoaiTruong.LaChuyenGia;
                            Data.FileDinhKem = _FileDinhKemBUS.GetByLoaiFileAndNghiepVuIDAndCoQuanID(EnumLoaiFileDinhKem.LyLich.GetHashCode(), Data.CanBoID, Data.CoQuanID);
                            if (Data.FileDinhKem != null && Data.FileDinhKem.Count > 0)
                            {
                                foreach (var item in Data.FileDinhKem)
                                {
                                    item.FileUrl = serverPath + item.FileUrl;
                                }
                            }
                            //Data.DuAnDeTai = _LyLichKhoaHocBUS.GetDuAnDeTaiByCanBoID(Data.CanBoID);
                            Data.LaCanBoTrongTruong = false;
                            Data.NguoiGioiThieu = _LyLichKhoaHocBUS.GetNguoiGioiThieu(Data.CanBoID, Data.CoQuanID);
                        }
                    }
                    //thêm DuAnDeTai
                    var listDuAn = _LyLichKhoaHocBUS.GetDuAnDeTaiByCanBoID(Data.CanBoID);
                    if (listDuAn.Count > 0)
                    {
                        if (Data.DuAnDeTai == null) Data.DuAnDeTai = new List<DuAnDeTaiModel>();
                        Data.DuAnDeTai.AddRange(listDuAn);
                    }
                    //thêm vai trò trong DuAnDeTai
                    var listVaiTro = rsApiInCore.core_getConversions(0);
                    if (Data.DuAnDeTai != null && Data.DuAnDeTai.Count > 0)
                    {
                        foreach (var item in Data.DuAnDeTai)
                        {
                            if (item.VaiTro > 0)
                            {
                                for (int i = 0; i < listVaiTro.Count; i++)
                                {
                                    if (item.VaiTro == listVaiTro[i].Id)
                                    {
                                        item.VaiTroThamGia = listVaiTro[i].Name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    try
                    {
                        //Boolean LaCoQuanTrongTruong = Data.CoQuanID != coQuanNgoaiTruong ? true : false;
                        //var ThongTinThemNKH = _LyLichKhoaHocBUS.GetThongTinNhaKhoaHoc_DeTai(Data.CanBoID, LaCoQuanTrongTruong);
                        //if (ThongTinThemNKH.BaiBaoTapChi != null)
                        //{
                        //    if (Data.BaiBaoTapChi == null) Data.BaiBaoTapChi = new List<BaiBaoTapChiModel>();
                        //    Data.BaiBaoTapChi.AddRange(ThongTinThemNKH.BaiBaoTapChi);
                        //}
                        //if (ThongTinThemNKH.SachChuyenKhao != null)
                        //{
                        //    if (Data.SachChuyenKhao == null) Data.SachChuyenKhao = new List<SachChuyenKhaoModel>();
                        //    Data.SachChuyenKhao.AddRange(ThongTinThemNKH.SachChuyenKhao);
                        //}
                        //if (ThongTinThemNKH.KetQuaNghienCuu != null)
                        //{
                        //    if (Data.KetQuaNghienCuu == null) Data.KetQuaNghienCuu = new List<KetQuaNghienCuuNKHModel>();
                        //    Data.KetQuaNghienCuu.AddRange(ThongTinThemNKH.KetQuaNghienCuu);
                        //}

                        //thêm hoạt động khoa học khác trường hợp không có thông tin chi tiết nhà khoa học
                        if (Data.HoatDongKhoaHoc == null || Data.HoatDongKhoaHoc.Count == 0)
                        {
                            Data.HoatDongKhoaHoc = new List<HoatDongKhoaHocModel>();
                            var HoatDongKhoaHocKhac = _LyLichKhoaHocBUS.HoatDongKhoaHoc_GetByCanBoID(Data.CanBoID, Data.CoQuanID);
                            if (HoatDongKhoaHocKhac.Count > 0)
                            {
                                foreach (var item in HoatDongKhoaHocKhac)
                                {
                                    if (item.FileDinhKem!=null && item.FileDinhKem.Count>0)
                                    {
                                        foreach (var i in item.FileDinhKem)
                                        {
                                            i.FileUrl = serverPath + i.FileUrl;
                                        }
                                    }
                                }
                                Data.HoatDongKhoaHoc.AddRange(HoatDongKhoaHocKhac);
                            }
                        }
                        //thêm thông tin bài báo, sách, sản phẩm từ nhà khoa học khác
                        if (Data.BaiBaoTapChi == null) Data.BaiBaoTapChi = new List<BaiBaoTapChiModel>();
                        if (Data.KetQuaNghienCuu == null) Data.KetQuaNghienCuu = new List<KetQuaNghienCuuNKHModel>();
                        if (Data.SachChuyenKhao == null) Data.SachChuyenKhao = new List<SachChuyenKhaoModel>();
                        if (Data.SanPhamDaoTao == null) Data.SanPhamDaoTao = new List<SanPhamDaoTaoModel>();

                        var ThongTinThemTuNKH = _LyLichKhoaHocBUS.GetThongTinTuNhaKhoaHocKhac(Data.CanBoID, Data.CoQuanID);
                        if (ThongTinThemTuNKH.BaiBaoTapChi != null && ThongTinThemTuNKH.BaiBaoTapChi.Count > 0)
                        { 
                            Data.BaiBaoTapChi.AddRange(ThongTinThemTuNKH.BaiBaoTapChi);
                        }
                        if (ThongTinThemTuNKH.KetQuaNghienCuu != null && ThongTinThemTuNKH.KetQuaNghienCuu.Count > 0)
                        { 
                            Data.KetQuaNghienCuu.AddRange(ThongTinThemTuNKH.KetQuaNghienCuu);
                        }
                        if (ThongTinThemTuNKH.SachChuyenKhao != null && ThongTinThemTuNKH.SachChuyenKhao.Count > 0)
                        {
                            Data.SachChuyenKhao.AddRange(ThongTinThemTuNKH.SachChuyenKhao);
                        }
                        //thêm thông tin bài báo, sách, sản phẩm từ đề tài
                        var ThongTinThemTuDeTai = _LyLichKhoaHocBUS.GetThongTinTuDeTai(Data.CanBoID, Data.CoQuanID);
                        if (ThongTinThemTuDeTai.BaiBaoTapChi != null && ThongTinThemTuDeTai.BaiBaoTapChi.Count > 0)
                        { 
                            Data.BaiBaoTapChi.AddRange(ThongTinThemTuDeTai.BaiBaoTapChi);
                        }
                        if (ThongTinThemTuDeTai.KetQuaNghienCuu != null && ThongTinThemTuDeTai.KetQuaNghienCuu.Count > 0)
                        {  
                            Data.KetQuaNghienCuu.AddRange(ThongTinThemTuDeTai.KetQuaNghienCuu);
                        }
                        if (ThongTinThemTuDeTai.SachChuyenKhao != null && ThongTinThemTuDeTai.SachChuyenKhao.Count > 0)
                        { 
                            Data.SachChuyenKhao.AddRange(ThongTinThemTuDeTai.SachChuyenKhao);
                        }
                        if (ThongTinThemTuDeTai.SanPhamDaoTao != null && ThongTinThemTuDeTai.SanPhamDaoTao.Count > 0)
                        {
                            Data.SanPhamDaoTao.AddRange(ThongTinThemTuDeTai.SanPhamDaoTao);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }


                    //if (Data.BaiBaoTapChi != null && Data.BaiBaoTapChi.Count > 0)
                    //{
                    //    foreach (var item in Data.BaiBaoTapChi)
                    //    {
                    //        if(item.ListTacGia != null && item.ListTacGia.Count > 0)
                    //        {
                    //            foreach (var tg in item.ListTacGia)
                    //            {
                    //                if(tg.TenTacGia != null && tg.TenTacGia.Length > 0)
                    //                {

                    //                }
                    //                else
                    //                {

                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    base.Status = 1;
                    base.Data = Data;
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

        [HttpPost]
        [Route("NhaKhoaHoc_ChinhSuaThongTinChiTiet")]
        //[CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Edit)]
        public async Task<IActionResult> NhaKhoaHoc_ChinhSuaThongTinChiTiet(IList<IFormFile> files, [FromForm] string ThongTinChiTiet)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, async () =>
                //{
                var ChiTiet = JsonConvert.DeserializeObject<ThongTinCTNhaKhoaHocModel>(ThongTinChiTiet);
                var Result = _LyLichKhoaHocBUS.Edit_ThongTinChiTiet(ChiTiet);
                int NghiepVuID = Utils.ConvertToInt32(Result.Data, 0);
                if (NghiepVuID > 0 && files != null && files.Count > 0)
                {
                    try
                    {
                        var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                        var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    }
                    catch (Exception)
                    {
                        Result.Message = "token expiry";
                    }

                    var clsCommon = new Commons();
                    //Thêm mới file
                    if (ChiTiet.FileDinhKem != null && ChiTiet.FileDinhKem.Count > 0)
                    {
                        foreach (var item in ChiTiet.FileDinhKem)
                        {
                            if (item.TenFileGoc != null && item.TenFileGoc.Length > 0)
                            {
                                item.NghiepVuID = NghiepVuID;
                                item.NguoiTaoID = CanBoID;
                                item.CoQuanID = ChiTiet.CoQuanID;
                                foreach (IFormFile source in files)
                                {
                                    if (source.FileName == item.TenFileGoc)
                                    {
                                        await clsCommon.InsertFileAsync(source, item, _host, _FileDinhKemBUS);
                                    }
                                }
                            }
                        }
                    }
                }
                if (NghiepVuID > 0)
                {
                    //Delete File
                    if (ChiTiet.DeleteFileDinhKem != null && ChiTiet.DeleteFileDinhKem.Count > 0)
                    {
                        foreach (int FileDinhKemID in ChiTiet.DeleteFileDinhKem)
                        {
                            var del = _FileDinhKemBUS.Delete_FileDinhKemID(FileDinhKemID);
                        }
                    }
                }
                base.Data = Result.Data;
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
                //});
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }

        }

        [HttpPost]
        [Route("NhaKhoaHoc_XoaThongTinChiTiet")]
        [CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Delete)]
        public IActionResult NhaKhoaHoc_XoaThongTinChiTiet(ThongTinCTNhaKhoaHocModel ChiTiet)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _LyLichKhoaHocBUS.Delete_ThongTinChiTiet(ChiTiet);
                    base.Data = Result.Data;
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
                throw ex;
            }

        }

        [HttpPost]
        [Route("NhaKhoaHoc_ChinhSuaHoatDongKhoaHoc")]
        //[CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Edit)]
        public async Task<IActionResult> NhaKhoaHoc_ChinhSuaHoatDongKhoaHoc(IList<IFormFile> files, [FromForm] string HoatDongKhoaHoc)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, async () =>
                //{
                var CanBoID = 0;
                var CoQuanID = 0;
                try
                {
                    CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                }
                catch (Exception) { }

                var hdkh = JsonConvert.DeserializeObject<HoatDongKhoaHocModel>(HoatDongKhoaHoc);
                //hdkh.CoQuanID = CoQuanID;
                var Result = _LyLichKhoaHocBUS.Edit_HoatDongKhoaHoc(hdkh);
                int NghiepVuID = Utils.ConvertToInt32(Result.Data, 0);
                if (NghiepVuID > 0 && files != null && files.Count > 0)
                {
                    var clsCommon = new Commons();
                    if (hdkh.FileDinhKem != null && hdkh.FileDinhKem.Count > 0)
                    {
                        foreach (var item in hdkh.FileDinhKem)
                        {
                            if (item.TenFileGoc != null && item.TenFileGoc.Length > 0)
                            {
                                item.NghiepVuID = NghiepVuID;
                                item.NguoiTaoID = CanBoID;
                                item.CoQuanID = hdkh.CoQuanID;
                                foreach (IFormFile source in files)
                                {
                                    if (source.FileName == item.TenFileGoc)
                                    {
                                        await clsCommon.InsertFileAsync(source, item, _host, _FileDinhKemBUS);
                                    }
                                }
                            }
                        }
                    }
                }
                if (NghiepVuID > 0)
                {
                    //Delete File
                    if (hdkh.DeleteFileDinhKem != null && hdkh.DeleteFileDinhKem.Count > 0)
                    {
                        foreach (int FileDinhKemID in hdkh.DeleteFileDinhKem)
                        {
                            var del = _FileDinhKemBUS.Delete_FileDinhKemID(FileDinhKemID);
                        }
                    }
                }
                base.Data = Result.Data;
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
                //});
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }

        }

        [HttpPost]
        [Route("NhaKhoaHoc_XoaHoatDongKhoaHoc")]
        [CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Delete)]
        public IActionResult NhaKhoaHoc_XoaHoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _LyLichKhoaHocBUS.Delete_HoatDongKhoaHoc(HoatDongKhoaHoc);
                    base.Data = Result.Data;
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
                throw ex;
            }

        }

        [HttpPost]
        [Route("NhaKhoaHoc_CapNhat")]
        //[CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Edit)]
        public IActionResult NhaKhoaHoc_CapNhat(NhaKhoaHocModel NhaKhoaHocModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = new BaseResultModel();
                    if (NhaKhoaHocModel.CoQuanID == Utils.ConvertToInt32(new SystemConfigDAL().GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999))
                    {
                        Result = _LyLichKhoaHocBUS.Update(NhaKhoaHocModel);
                    }
                    else Result = _LyLichKhoaHocBUS.UpdateURL(NhaKhoaHocModel);
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
                throw ex;
            }

        }

        [HttpPost]
        [Route("NhaKhoaHoc_CapNhat_AnhDaiDien")]
        [CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Edit)]
        public async Task<IActionResult> NhaKhoaHoc_CapNhat_AnhDaiDien(IFormFile AnhDaiDien, [FromForm] string NoiDung)
        {
            try
            {
                var FileDinhKem = JsonConvert.DeserializeObject<FileDinhKemModel>(NoiDung);
                int CanBoID = FileDinhKem.NghiepVuID;
                int CoQuanID = Utils.ConvertToInt32(FileDinhKem.CoQuanID, 0);
                //delete file đính kèm có sẵn
                int loaiFile = EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode();
                var ResultDeleteFile = _FileDinhKemBUS.Delete(loaiFile, CanBoID);
                // thêm bảng file đính kèm sau khi delete thành công
                if (ResultDeleteFile.Status > 0)
                {
                    var CanBoDangNhapID = 0;
                    try
                    {
                        CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    }
                    catch (Exception)
                    {
                        base.Message = "token expiry";
                    }
                    var clsCommon = new Commons();
                    FileDinhKemModel fileData = new FileDinhKemModel();
                    fileData.LoaiFile = EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode();
                    fileData.NguoiTaoID = CanBoDangNhapID;
                    fileData.NghiepVuID = CanBoID;
                    fileData.NoiDung = string.Empty;
                    fileData.CoQuanID = CoQuanID;
                    fileData.FolderPath = nameof(EnumLoaiFileDinhKem.AnhDaiDien);

                    var insertFile = await clsCommon.InsertFileAsync(AnhDaiDien, fileData, _host, _FileDinhKemBUS);
                    if (insertFile)
                    {
                        base.Status = 1;
                        base.Message = "Cập nhật ảnh đại diện thành công";
                    }
                    else
                    {
                        base.Status = 0;
                        base.Message = "Không thể cập nhật ảnh đại diện";
                    }
                }
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }
        }


        /// <summary>
        /// trả về chi tiết lý lịch khoa học trong trường cho website
        /// </summary>
        /// <param name="StaffId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ChiTiet")]
        //[CustomAuthAttribute(ChucNangEnum.chi_tiet_nha_khoa_hoc, AccessLevel.Read)]
        public IActionResult ChiTiet(int StaffId)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var canBoInfo = rsApiInCore.core_getstaff(StaffId);
                    NhaKhoaHocModel Data = new NhaKhoaHocModel();
                    Data = _LyLichKhoaHocBUS.GetByID(StaffId, serverPath, canBoInfo.DepartmentId);
                    Data.AnhHoSo = (Data.AnhHoSo != null && Data.AnhHoSo != "" && Data.AnhHoSo != string.Empty) ? (serverPath + Data.AnhHoSo) : "";

                    // cơ quan trong trường
                    Data.CanBoID = canBoInfo.Id;
                    Data.TenCanBo = canBoInfo.Name;
                    Data.MaCB = canBoInfo.Code;
                    Data.NgaySinh = canBoInfo.Birthday;
                    Data.CoQuanID = canBoInfo.DepartmentId;
                    Data.PhongBanID = canBoInfo.DepartmentId;
                    Data.Email = canBoInfo.Email;
                    Data.CoQuanCongTac = "";
                    Data.GioiTinhStr = canBoInfo.Gender;
                    Data.ChucDanhKhoaHoc = new List<int>() { canBoInfo.DegreeId };
                    Data.ChucDanhHanhChinh = canBoInfo.PositionIds.ToList();
                    Data.LaCanBoTrongTruong = true;
                    var FileDinhKem = _FileDinhKemBUS.GetByLoaiFileAndNghiepVuIDAndCoQuanID(EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode(), Data.CanBoID, Data.CoQuanID);
                    if (FileDinhKem != null && FileDinhKem.Count > 0)
                    {
                        foreach (var item in FileDinhKem)
                        {
                            item.FileUrl = serverPath + item.FileUrl;
                            Data.AnhHoSo = item.FileUrl;
                        }
                    }
                    Data.Url = _LyLichKhoaHocBUS.GetUrlCanBoTrongTruong(Data.CanBoID, Data.CoQuanID);
                    Data.TrangThaiID = 0;
                    if (canBoInfo.IsMoved)
                    {
                        Data.TrangThaiID = 1;
                    }
                    if (canBoInfo.IsProbation)
                    {
                        Data.TrangThaiID = 2;
                    }
                    if (canBoInfo.IsRetired)
                    {
                        Data.TrangThaiID = 3;
                    }

                    //thêm DuAnDeTai
                    var listDuAn = _LyLichKhoaHocBUS.GetDuAnDeTaiByCanBoID(Data.CanBoID);
                    if (listDuAn.Count > 0)
                    {
                        if (Data.DuAnDeTai == null) Data.DuAnDeTai = new List<DuAnDeTaiModel>();
                        Data.DuAnDeTai.AddRange(listDuAn);
                    }
                    //thêm vai trò trong DuAnDeTai
                    var listVaiTro = rsApiInCore.core_getConversions(0);
                    if (Data.DuAnDeTai != null && Data.DuAnDeTai.Count > 0)
                    {
                        foreach (var item in Data.DuAnDeTai)
                        {
                            if (item.VaiTro > 0)
                            {
                                for (int i = 0; i < listVaiTro.Count; i++)
                                {
                                    if (item.VaiTro == listVaiTro[i].Id)
                                    {
                                        item.VaiTroThamGia = listVaiTro[i].Name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    try
                    {
                        var ThongTinThemNKH = _LyLichKhoaHocBUS.GetThongTinNhaKhoaHoc_DeTai(Data.CanBoID, true);
                        if (ThongTinThemNKH.BaiBaoTapChi != null)
                        {
                            if (Data.BaiBaoTapChi == null) Data.BaiBaoTapChi = new List<BaiBaoTapChiModel>();
                            Data.BaiBaoTapChi.AddRange(ThongTinThemNKH.BaiBaoTapChi);
                        }
                        if (ThongTinThemNKH.SachChuyenKhao != null)
                        {
                            if (Data.SachChuyenKhao == null) Data.SachChuyenKhao = new List<SachChuyenKhaoModel>();
                            Data.SachChuyenKhao.AddRange(ThongTinThemNKH.SachChuyenKhao);
                        }
                        if (ThongTinThemNKH.KetQuaNghienCuu != null)
                        {
                            if (Data.KetQuaNghienCuu == null) Data.KetQuaNghienCuu = new List<KetQuaNghienCuuNKHModel>();
                            Data.KetQuaNghienCuu.AddRange(ThongTinThemNKH.KetQuaNghienCuu);
                        }
                        //thêm hoạt động khoa học khác trường hợp không có thông tin chi tiết nhà khoa học
                        if (Data.HoatDongKhoaHoc == null || Data.HoatDongKhoaHoc.Count == 0)
                        {
                            Data.HoatDongKhoaHoc = new List<HoatDongKhoaHocModel>();
                            var HoatDongKhoaHocKhac = _LyLichKhoaHocBUS.HoatDongKhoaHoc_GetByCanBoID(Data.CanBoID, Data.CoQuanID);
                            if (HoatDongKhoaHocKhac.Count > 0)
                            {
                                Data.HoatDongKhoaHoc.AddRange(HoatDongKhoaHocKhac);
                            }
                        }

                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    base.Status = 1;
                    base.Data = Data;
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