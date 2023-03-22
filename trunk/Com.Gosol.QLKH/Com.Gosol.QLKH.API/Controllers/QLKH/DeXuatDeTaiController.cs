using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
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

namespace Com.Gosol.QLKH.API.Controllers
{
    [Route("api/v1/DeXuatDeTai")]
    [ApiController]
    public class DeXuatDeTaiController : BaseApiController
    {
        private IDeXuatDeTaiBUS _DeXuatDeTaiBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IPhanQuyenBUS _PhanQuyenBUS;
        private IQuanLyThongBaoBUS _QuanLyThongBaoBUS;
        private ISystemConfigBUS _SystemConfigBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;
        private IMemoryCache _cache;
        public DeXuatDeTaiController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, ISystemConfigBUS SystemConfigBUS, IDeXuatDeTaiBUS DeXuatDeTaiBUS, IFileDinhKemBUS FileDinhKemBUS, IPhanQuyenBUS PhanQuyenBUS, IQuanLyThongBaoBUS QuanLyThongBaoBUS, ILogHelper _LogHelper, ILogger<DeXuatDeTaiController> logger) : base(_LogHelper, logger)
        {
            this._DeXuatDeTaiBUS = DeXuatDeTaiBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            this._PhanQuyenBUS = PhanQuyenBUS;
            this._QuanLyThongBaoBUS = QuanLyThongBaoBUS;
            this._SystemConfigBUS = SystemConfigBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
            this._cache = memoryCache;
        }

        [HttpGet]
        [Route("DeXuatDeTai_DanhSach")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Read)]
        public IActionResult DeXuatDeTai_DanhSach([FromQuery] BasePagingParams p, int? LinhVucNghienCuu, int? LinhVucKinhTeXaHoi, int? CapQuanLy, int? TrangThai, int? CanBoID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
  
                    //int quyen = _PhanQuyenBUS.CheckQuyen(NguoiDungID);
                    int TotalRow = 0;
                    List<DeXuatDeTaiModel> listDeXuat = new List<DeXuatDeTaiModel>();
                    //if (quyen == EnumQuyenQuanLy.QuanLy.GetHashCode())
                    //{
                    //    listDeXuat = _DeXuatDeTaiBUS.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu ?? 0, LinhVucKinhTeXaHoi ?? 0, CapQuanLy ?? 0, TrangThai ?? 0, CanBoID ?? 0, 0, true, CanBoIDLogin);
                    //}
                    //else if(quyen == EnumQuyenQuanLy.TruongKhoa.GetHashCode())
                    //{
                    //    listDeXuat = _DeXuatDeTaiBUS.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu ?? 0, LinhVucKinhTeXaHoi ?? 0, CapQuanLy ?? 0, TrangThai ?? 0, CanBoID ?? 0, CoQuanID, true, CanBoIDLogin);
                    //}
                    //else listDeXuat = _DeXuatDeTaiBUS.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu ?? 0, LinhVucKinhTeXaHoi ?? 0, CapQuanLy ?? 0, TrangThai ?? 0, CanBoIDLogin, CoQuanID, false, CanBoIDLogin);

                    var QuanLy = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_QLKH").ConfigValue, 0);
                    var TruongKhoa = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_TRUONG_KHOA").ConfigValue, 0);
                    var NhaKhoaHoc = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_NKH").ConfigValue, 0);
                    if (p.Role == QuanLy)
                    {
                        listDeXuat = _DeXuatDeTaiBUS.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu ?? 0, LinhVucKinhTeXaHoi ?? 0, CapQuanLy ?? 0, TrangThai ?? 0, CanBoID ?? 0, 0, true, CanBoIDLogin);
                    }
                    else if (p.Role == TruongKhoa)
                    {
                        listDeXuat = _DeXuatDeTaiBUS.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu ?? 0, LinhVucKinhTeXaHoi ?? 0, CapQuanLy ?? 0, TrangThai ?? 0, CanBoID ?? 0, CoQuanID, true, CanBoIDLogin);
                    }
                    else listDeXuat = _DeXuatDeTaiBUS.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu ?? 0, LinhVucKinhTeXaHoi ?? 0, CapQuanLy ?? 0, TrangThai ?? 0, CanBoIDLogin, CoQuanID, false, CanBoIDLogin);

                    //thêm thông tin tên người đề xuất
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    if (result.Count > 0)
                    {
                        foreach (var item in listDeXuat)
                        {
                            if (item.TenNguoiDeXuat == null || item.TenNguoiDeXuat == "")
                            {
                                for (int i = 0; i < result.Count; i++)
                                {
                                    if (item.NguoiDeXuat == result[i].CanBoID)
                                    {
                                        item.TenNguoiDeXuat = result[i].TenCanBo;
                                        break;
                                    }
                                }
                            }
                        }

                        var clsCommon = new Commons();
                        string serverPath = clsCommon.GetServerPath(HttpContext);
                        foreach (var item in listDeXuat)
                        {    
                            foreach (var f in item.FileDinhKem)
                            {   
                                f.FileUrl = serverPath + f.FileUrl;
                            }
                        }
                    }

                    base.Status = 1;
                    base.TotalRow = TotalRow;
                    base.Data = listDeXuat;
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

        [HttpGet]
        [Route("DeXuatDeTai_ChiTiet")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Read)]
        public IActionResult DeXuatDeTai_ChiTiet(int DeXuatID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    DeXuatDeTaiModel Data;
                    Data = _DeXuatDeTaiBUS.GetByID(DeXuatID);
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    if (Data.FileDinhKem != null && Data.FileDinhKem.Count > 0)
                    {
                        var clsCommon = new Commons();
                        string serverPath = clsCommon.GetServerPath(HttpContext);
                        foreach (var item in Data.FileDinhKem)
                        {
                            item.FileUrl = item.FileUrl.Replace(@"\\", @"\");
                            item.FileUrl = serverPath + item.FileUrl;

                            if (item.TenNguoiTao == null || item.TenNguoiTao == "")
                            {
                                for (int i = 0; i < result.Count; i++)
                                {
                                    if (item.NguoiTaoID == result[i].CanBoID)
                                    {
                                        item.TenNguoiTao = result[i].TenCanBo;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (Data.ThongTinXetDuyetDeTai != null && Data.ThongTinXetDuyetDeTai.Count > 0)
                    {
                        var clsCommon = new Commons();
                        string serverPath = clsCommon.GetServerPath(HttpContext);
                        foreach (var item in Data.ThongTinXetDuyetDeTai)
                        {
                            if (item.FileXetDuyet != null && item.FileXetDuyet.Count > 0)
                            {
                                foreach (var f in item.FileXetDuyet)
                                {
                                    f.FileUrl = f.FileUrl.Replace(@"\\", @"\");
                                    f.FileUrl = serverPath + f.FileUrl;
                                }
                            }

                            if (item.TenNguoiThucHien == null || item.TenNguoiThucHien == "")
                            {
                                for (int i = 0; i < result.Count; i++)
                                {
                                    if (item.CanBoID == result[i].CanBoID)
                                    {
                                        item.TenNguoiThucHien = result[i].TenCanBo;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if(Data.ThongTinChinhSuaDeXuat.Count > 0)
                    {
                        foreach (var item in Data.ThongTinChinhSuaDeXuat)
                        {
                            //thêm tên ngươi chinh sửa
                            for (int i = 0; i < result.Count; i++)
                            {
                                if (item.CanBoID == result[i].CanBoID)
                                {
                                    item.NguoiChinhSua = result[i].TenCanBo;
                                    break;
                                }
                            }
                            //thêm tên người đề xuất
                            foreach (var dx in item.Data)
                            {
                                for (int i = 0; i < result.Count; i++)
                                {
                                    if (dx.NguoiDeXuat == result[i].CanBoID)
                                    {
                                        dx.TenNguoiDeXuat = result[i].TenCanBo;
                                        break;
                                    }
                                }
                            }   
                        }
                    }

                    base.Status = 1;
                    base.TotalRow = TotalRow;
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
        [Route("XoaFileDinhKem")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Delete)]
        public IActionResult XoaFileDinhKem(FileDinhKemModel FileDinhKemModel)
        {
            try
            {
                var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                var Result = _FileDinhKemBUS.Delete_FileDinhKemID(FileDinhKemModel.FileDinhKemID);
                var file = _FileDinhKemBUS.GetByID(FileDinhKemModel.FileDinhKemID);
                if (file.LoaiFile == (int)EnumLoaiFileDinhKem.DeXuatDeTai)
                {
                    _DeXuatDeTaiBUS.Update_DeXuatLog(file.NghiepVuID, CanBoID, CoQuanID);
                }
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

        [HttpPost]
        [Route("ThemMoiFileDinhKem")]
        //[CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Create)]
        public async Task<IActionResult> ThemMoiFileDinhKem(IList<IFormFile> files, [FromForm] string NoiDung)
        {
            try
            {
                var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                var FileDinhKem = JsonConvert.DeserializeObject<FileDinhKemModel>(NoiDung);
                FileDinhKem.NguoiTaoID = CanBoID;
                if (FileDinhKem.LoaiFile == (int)EnumLoaiFileDinhKem.LyLich)
                {
                    var coQuanNgoaiTruong = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                    FileDinhKem.CoQuanID = coQuanNgoaiTruong;
                }
                else FileDinhKem.CoQuanID = CoQuanID;

                if (FileDinhKem.LoaiFile == (int)EnumLoaiFileDinhKem.DeXuatDeTai || FileDinhKem.LoaiFile == (int)EnumLoaiFileDinhKem.DuyetDeXuat)
                {
                    FileDinhKem.FolderPath = nameof(EnumLoaiFileDinhKem.DeXuatDeTai);
                }
                var clsCommon = new Commons();
                foreach (IFormFile source in files)
                {
                    await clsCommon.InsertFileAsync(source, FileDinhKem, _host, _FileDinhKemBUS);
                }
                //Log đề xuất đề tài
                if (FileDinhKem.LoaiFile == (int)EnumLoaiFileDinhKem.DeXuatDeTai)
                {
                    _DeXuatDeTaiBUS.Update_DeXuatLog(FileDinhKem.NghiepVuID, CanBoID, CoQuanID);
                }
                //lấy file duyệt đề xuất sang đề tài
                //if (FileDinhKem.LoaiFile == (int)EnumLoaiFileDinhKem.DuyetDeXuat && files.Count > 0)
                //{
                //    var dt = _DeXuatDeTaiBUS.GetDeTaiByLSDeXuatID(FileDinhKem.NghiepVuID);
                //    if(dt.DeTaiID > 0)
                //    {
                //        FileDinhKem.FolderPath = nameof(EnumLoaiFileDinhKem.DeTai);
                //        FileDinhKem.NghiepVuID = dt.DeTaiID;
                //        FileDinhKem.LoaiFile = EnumLoaiFileDinhKem.DeTai.GetHashCode();
                //        foreach (IFormFile source in files)
                //        {
                //            await clsCommon.InsertFileAsync(source, FileDinhKem, _host, _FileDinhKemBUS);
                //        }
                //    }
                //}

                base.Status = 1;
                base.Message = "Thêm file thành công";
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
        [Route("DeXuatDeTai_ThemMoi")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Create)]
        public async Task<IActionResult> DeXuatDeTai_ThemMoi(IList<IFormFile> files, [FromForm] string ThongTinDeTai)
        {
            try
            {
                var DeTaiModel = JsonConvert.DeserializeObject<DeXuatDeTaiModel>(ThongTinDeTai);
                var Result = _DeXuatDeTaiBUS.Insert(DeTaiModel);
                int DeTaiID = Utils.ConvertToInt32(Result.Data, 0);
                //insert file đính kèm
                if (DeTaiID > 0 && files != null && files.Count > 0)
                {
                    var CanBoID = 0;
                    var CoQuanID = 0;
                    try
                    {
                        CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                        CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    }
                    catch (Exception)
                    {
                        Result.Message = "token expiry";
                    }

                    var clsCommon = new Commons();
                    if (DeTaiModel.FileDinhKem != null && DeTaiModel.FileDinhKem.Count > 0)
                    {
                        foreach (var item in DeTaiModel.FileDinhKem)
                        {
                            if (item.TenFileGoc != null && item.TenFileGoc.Length > 0)
                            {
                                string[] listTenFile = item.TenFileGoc.Split(';');
                                if (listTenFile.Length > 0)
                                {
                                    foreach (var tenFile in listTenFile)
                                    {
                                        if (tenFile != "")
                                        {
                                            FileDinhKemModel fileDinhKem = new FileDinhKemModel();
                                            fileDinhKem.TenFileGoc = tenFile;
                                            fileDinhKem.NoiDung = item.NoiDung;
                                            fileDinhKem.NghiepVuID = DeTaiID;
                                            fileDinhKem.LoaiFile = EnumLoaiFileDinhKem.DeXuatDeTai.GetHashCode();
                                            fileDinhKem.NguoiTaoID = CanBoID;
                                            fileDinhKem.CoQuanID = CoQuanID;
                                            fileDinhKem.FolderPath = nameof(EnumLoaiFileDinhKem.DeXuatDeTai);
                                            foreach (IFormFile source in files)
                                            {
                                                if (source.FileName == fileDinhKem.TenFileGoc)
                                                {
                                                    await clsCommon.InsertFileAsync(source, fileDinhKem, _host, _FileDinhKemBUS);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
        [Route("DeXuatDeTai_CapNhat")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Edit)]
        public IActionResult DeXuatDeTai_CapNhat(DeXuatDeTaiModel DeTaiModel)
        {
            try
            {
                var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                DeTaiModel.CanBoChinhSuaID = CanBoID;
                DeTaiModel.CoQuanChinhSuaID = CoQuanID;
                var Result = _DeXuatDeTaiBUS.Update(DeTaiModel);
                _DeXuatDeTaiBUS.Update_DeXuatLog(DeTaiModel.DeXuatID, CanBoID, CoQuanID);
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

        [HttpPost]
        [Route("DeXuatDeTai_CapNhatTrangThai")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Edit)]
        public IActionResult DeXuatDeTai_CapNhatTrangThai(LichSuDuyetDeXuatModel DeTaiModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var CanBoID = 0;
                    var CoQuanID = 0;
                    try
                    {
                        CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                        CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    }
                    catch (Exception)
                    {

                    }
                    DeTaiModel.CanBoID = CanBoID;
                    DeTaiModel.CoQuanID = CoQuanID;
                    var Result = _DeXuatDeTaiBUS.Update_TrangThaiDeTai(DeTaiModel);
                    if(Result.Status == 1)
                    {
                        DeXuatDeTaiModel DeXuatModel = _DeXuatDeTaiBUS.GetByID(DeTaiModel.DeXuatID);
                        if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.ChuaDuyet.GetHashCode())
                        {
                            //Gửi thông báo cho quản lý
                            QuanLyThongBaoModel tb = new QuanLyThongBaoModel();
                            tb.TenThongBao = "Có đề xuất mới chưa duyệt";
                            tb.NoiDung = "Đề xuất \"" + DeXuatModel.TenDeXuat + "\" chưa duyệt";

                            tb.ThoiGianBatDau = DateTime.Now;
                            tb.ThoiGianKetThuc = DateTime.Now;
                            tb.HienThi = true;
                            tb.LoaiThongBao = EnumLoaiThongBao.DeXuat.GetHashCode();
                            tb.DoiTuongThongBao = new List<DoiTuongThongBaoModel>();
                            var listDoiTuong = _DeXuatDeTaiBUS.GetListQuanLy(CoQuanID);
                            var result = this.rsApiInCore.core_getusers(CoQuanID);
                            foreach (var item in listDoiTuong)
                            {
                                if(item.CanBoID > 0)
                                {
                                    DoiTuongThongBaoModel dt = new DoiTuongThongBaoModel();
                                    dt.CanBoID = item.CanBoID;
                                    dt.CoQuanID = item.CoQuanID;
                                    tb.DoiTuongThongBao.Add(dt);
                                }
                                else
                                {
                                    if(result != null && result.Count > 0)
                                    {
                                        foreach (var nd in result)
                                        {
                                            if(item.NguoiDungID == nd.Id)
                                            {
                                                DoiTuongThongBaoModel dt = new DoiTuongThongBaoModel();
                                                dt.CanBoID = nd.StaffId;
                                                dt.CoQuanID = item.CoQuanID;
                                                tb.DoiTuongThongBao.Add(dt);
                                            }
                                        }
                                    }
                                }
                            }
                            _QuanLyThongBaoBUS.Edit_ThongBao(tb);
                        }
                        if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.DuyetPhaiSua.GetHashCode()
                            || DeTaiModel.TrangThai == EnumTrangThaiDeXuat.DaDuyet.GetHashCode()
                            || DeTaiModel.TrangThai == EnumTrangThaiDeXuat.KhongDuyet.GetHashCode()
                            || DeTaiModel.TrangThai == EnumTrangThaiDeXuat.ChoDuyet.GetHashCode())
                        {
                            //Gửi thông báo cho nhà khoa học
                            QuanLyThongBaoModel tb = new QuanLyThongBaoModel();
                            if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.DuyetPhaiSua.GetHashCode())
                            {
                                tb.TenThongBao = "Có đề xuất duyệt phải sửa";
                                tb.NoiDung = "Đề xuất \"" + DeXuatModel.TenDeXuat + "\" duyệt phải sửa";
                            }
                            else if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.DaDuyet.GetHashCode())
                            {
                                tb.TenThongBao = "Có đề xuất đã duyệt";
                                tb.NoiDung = "Đề xuất \"" + DeXuatModel.TenDeXuat + "\" đã duyệt";
                            }
                            else if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.KhongDuyet.GetHashCode())
                            {
                                tb.TenThongBao = "Có đề xuất không duyệt";
                                tb.NoiDung = "Đề xuất \"" + DeXuatModel.TenDeXuat + "\" không duyệt";
                            }
                            else if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.ChoDuyet.GetHashCode())
                            {
                                tb.TenThongBao = "Có đề xuất chờ duyệt";
                                tb.NoiDung = "Đề xuất \"" + DeXuatModel.TenDeXuat + "\" chờ duyệt";
                            }
                          

                            tb.ThoiGianBatDau = DateTime.Now;
                            tb.ThoiGianKetThuc = DateTime.Now;
                            tb.HienThi = true;
                            tb.LoaiThongBao = EnumLoaiThongBao.DeXuat.GetHashCode();
                            tb.DoiTuongThongBao = new List<DoiTuongThongBaoModel>();
                            DoiTuongThongBaoModel dt = new DoiTuongThongBaoModel();
                            dt.CanBoID = DeXuatModel.NguoiDeXuat;
                            dt.CoQuanID = DeXuatModel.CoQuanID;
                            tb.DoiTuongThongBao.Add(dt);
                            _QuanLyThongBaoBUS.Edit_ThongBao(tb);
                        }
                    }
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
        [Route("DeXuatDeTai_XoaDeXuat")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_xuat, AccessLevel.Delete)]
        public IActionResult DeXuatDeTai_XoaDeXuat(DeXuatDeTaiModel DeXuatDeTaiModel)
        {
            try
            {
                var Result = _DeXuatDeTaiBUS.Delete(DeXuatDeTaiModel);
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
    }
}