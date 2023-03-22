using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Com.Gosol.QLKH.API.Controllers.QLKH
{
    [Route("api/v1/DeTai")]
    [ApiController]
    public class DeTaiController : BaseApiController
    {
        private IDeTaiBUS _DeTaiBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IPhanQuyenBUS _PhanQuyenBUS;
        private IHeThongCanBoBUS _HeThongCanBoBUS;
        private ISystemConfigBUS _SystemConfigBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;
        private IMemoryCache _cache;
        public DeTaiController(IHeThongCanBoBUS HeThongCanBoBUS, IMemoryCache memoryCache, ISystemConfigBUS SystemConfigBUS, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, IDeTaiBUS DeTaiBUS, IFileDinhKemBUS FileDinhKemBUS, IPhanQuyenBUS PhanQuyenBUS, ILogHelper _LogHelper, ILogger<DeTaiController> logger) : base(_LogHelper, logger)
        {
            this._DeTaiBUS = DeTaiBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            this._PhanQuyenBUS = PhanQuyenBUS;
            this._HeThongCanBoBUS = HeThongCanBoBUS;
            this._SystemConfigBUS = SystemConfigBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
            _cache = memoryCache;
        }

        [HttpGet]
        [Route("DeTai_DanhSach")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Read)]
        public IActionResult DeTai_DanhSach([FromQuery] BasePagingParams p, int? LinhVucNghienCuu, int? LinhVucKinhTeXaHoi, int? CapQuanLy, int? TrangThai, int? CanBoID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    //Phân quyền quản lý
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var CanBoIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    int quyen = _PhanQuyenBUS.CheckQuyen(NguoiDungID);
                    List<int> listCanBo = new List<int>();
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);

                    var QuanLy = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_QLKH").ConfigValue, 0);
                    var TruongKhoa = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_TRUONG_KHOA").ConfigValue, 0);
                    var NhaKhoaHoc = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_NKH").ConfigValue, 0);

                    if (p.Role == QuanLy)
                    {
                        foreach (var item in result)
                        {
                            listCanBo.Add(item.CanBoID);
                        }
                    }
                    else if (p.Role == TruongKhoa)
                    {
                        int? CoQuanID = 0;
                        foreach (var item in result)
                        {
                            if (item.CanBoID == CanBoIDLogin)
                            {
                                CoQuanID = item.CoQuanID;
                                break;
                            }
                        }
                        foreach (var item in result)
                        {
                            if (item.CoQuanID == CoQuanID) listCanBo.Add(item.CanBoID);
                        }
                    }
                    else
                    {
                        listCanBo.Add(CanBoIDLogin);
                    }

                    int TotalRow = 0;
                    List<DeTaiModel> Data;
                    Data = _DeTaiBUS.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu ?? 0, LinhVucKinhTeXaHoi ?? 0, CapQuanLy ?? 0, TrangThai ?? 0, CanBoID ?? 0, listCanBo).ToList();
                    //thêm thông tin tên chủ đề tài
                    //if (result.Count > 0)
                    //{
                    //    Data.ForEach(x => x.TenChuNhiemDeTai = result.FirstOrDefault(i => i.CanBoID == x.ChuNhiemDeTaiID).TenCanBo);
                    //}
                    if (result.Count > 0)
                    {
                        foreach (var item in Data)
                        {
                            if (item.TenChuNhiemDeTai == null || item.TenChuNhiemDeTai == "")
                            {
                                for (int i = 0; i < result.Count; i++)
                                {
                                    if (item.ChuNhiemDeTaiID == result[i].CanBoID)
                                    {
                                        item.TenChuNhiemDeTai = result[i].TenCanBo;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    //thanh vien tham gia str
                    foreach (var item in Data)
                    {
                        string ThanhVienThamGia = "";
                        if (item.ThanhVienNghienCuuStr != null || item.ThanhVienNghienCuuStr.Length > 0)
                        {
                            var listThanhVien = item.ThanhVienNghienCuuStr.Split(',');
                            foreach (var tv in listThanhVien)
                            {
                                if (tv != null && tv.Length > 0)
                                {
                                    var listID = tv.Split('-');
                                    if (listID[1] != null && listID[1].Length > 0)
                                    {
                                        ThanhVienThamGia += listID[1] + "; ";
                                    }
                                    else
                                    {
                                        int id = Utils.ConvertToInt32(listID[0], 0);
                                        for (int i = 0; i < result.Count; i++)
                                        {
                                            if (id == result[i].CanBoID)
                                            {
                                                ThanhVienThamGia += result[i].TenCanBo + "; ";
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                            if (ThanhVienThamGia.Length > 2) item.ThanhVienNghienCuuStr = ThanhVienThamGia.Substring(0, ThanhVienThamGia.Length - 2);
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

        [HttpGet]
        [Route("DeTai_ChiTiet")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Read)]
        public IActionResult DeTai_ChiTiet(int DeTaiID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    DeTaiModel Data = _DeTaiBUS.GetByID(DeTaiID, serverPath);
                    //thêm thông tin chủ nhiệm đề tài
                    if (Data.ChuNhiemDeTaiID > 0)
                    {
                        var chucDanhs = rsApiInCore.core_getTitles();
                        var hocHamHocVis = rsApiInCore.core_getDegrees();
                        var coQuanNgoaiTruong = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                        if (Data.CoQuanChuNhiemID == coQuanNgoaiTruong)
                        {
                            var canBoNgoaiTruong = _HeThongCanBoBUS.GetCanBoByID(Data.ChuNhiemDeTaiID ?? 0, 0);
                            Data.CanBoData = new ThongTinChuNhiemDeTai();
                            Data.CanBoData.CanBoID = canBoNgoaiTruong.CanBoID;
                            Data.CanBoData.MaCanBo = canBoNgoaiTruong.MaCB;
                            Data.CanBoData.TenCanBo = canBoNgoaiTruong.TenCanBo;
                            Data.CanBoData.NgaySinh = canBoNgoaiTruong.NgaySinh;
                            Data.CanBoData.Email = canBoNgoaiTruong.Email;
                            Data.CanBoData.DienThoai = canBoNgoaiTruong.DienThoai;
                            Data.CanBoData.Fax = canBoNgoaiTruong.Fax;
                            Data.CanBoData.ChucDanhID = canBoNgoaiTruong.ChucDanhHanhChinh != null ? canBoNgoaiTruong.ChucDanhHanhChinh[0] : 0;
                            Data.CanBoData.HocHamHocViID = canBoNgoaiTruong.ChucDanhKhoaHoc != null ? canBoNgoaiTruong.ChucDanhKhoaHoc[0] : 0;
                            if (canBoNgoaiTruong.GioiTinh > 0)
                            {
                                Data.CanBoData.GioiTinhStr = "Nam";
                            }
                            else Data.CanBoData.GioiTinhStr = "Nữ";
                            if (canBoNgoaiTruong.ChucDanhHanhChinh != null && canBoNgoaiTruong.ChucDanhHanhChinh.Count > 0)
                            {
                                string ChucDanhHanhChinh = "";
                                foreach (var cd in canBoNgoaiTruong.ChucDanhHanhChinh)
                                {
                                    foreach (var item in chucDanhs)
                                    {
                                        if (item.Id == cd)
                                        {
                                            ChucDanhHanhChinh += item.Name + ", ";
                                            break;
                                        }
                                    }
                                }
                                if (ChucDanhHanhChinh.Length > 2) Data.CanBoData.TenChucDanh = ChucDanhHanhChinh.Substring(0, ChucDanhHanhChinh.Length - 2);
                            }
                            if (canBoNgoaiTruong.ChucDanhKhoaHoc != null && canBoNgoaiTruong.ChucDanhKhoaHoc.Count > 0)
                            {
                                string ChucDanhKhoaHoc = "";
                                foreach (var cd in canBoNgoaiTruong.ChucDanhKhoaHoc)
                                {
                                    foreach (var item in hocHamHocVis)
                                    {
                                        if (item.Id == cd)
                                        {
                                            ChucDanhKhoaHoc += item.Name + ", ";
                                            break;
                                        }
                                    }
                                }
                                if (ChucDanhKhoaHoc.Length > 2) Data.CanBoData.TenHocHamHocVi = ChucDanhKhoaHoc.Substring(0, ChucDanhKhoaHoc.Length - 2);
                            }
                        }
                        else
                        {
                            var thongTinChiTiet = rsApiInCore.core_getstaff(Data.ChuNhiemDeTaiID ?? 0);
                            Data.CanBoData = new ThongTinChuNhiemDeTai();
                            Data.CanBoData.CanBoID = thongTinChiTiet.Id;
                            Data.CanBoData.MaCanBo = thongTinChiTiet.Code;
                            Data.CanBoData.TenCanBo = thongTinChiTiet.Name;
                            Data.CanBoData.NgaySinh = thongTinChiTiet.Birthday;
                            Data.CanBoData.DiaChi = thongTinChiTiet.DiaChi;
                            Data.CanBoData.Email = thongTinChiTiet.Email;
                            Data.CanBoData.DienThoai = thongTinChiTiet.DienThoai;
                            Data.CanBoData.Fax = thongTinChiTiet.Fax;
                            Data.CanBoData.ChucDanhID = thongTinChiTiet.TitleId;
                            Data.CanBoData.HocHamHocViID = thongTinChiTiet.DegreeId;
                            Data.CanBoData.GioiTinhStr = thongTinChiTiet.Gender;
                            if (Data.CanBoData.NgaySinh != null) Data.CanBoData.NgaySinhStr = thongTinChiTiet.Birthday.ToString("dd/MM/yyyy");
                            if (Data.CanBoData.ChucDanhID > 0)
                            {
                                foreach (var item in chucDanhs)
                                {
                                    if (item.Id == Data.CanBoData.ChucDanhID)
                                    {
                                        Data.CanBoData.TenChucDanh = item.Name;
                                        break;
                                    }
                                }
                            }
                            if (Data.CanBoData.HocHamHocViID > 0)
                            {
                                foreach (var item in hocHamHocVis)
                                {
                                    if (item.Id == Data.CanBoData.HocHamHocViID)
                                    {
                                        Data.CanBoData.TenHocHamHocVi = item.Name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //thêm tên người tạo file
                    if (Data.FileDinhKem != null && Data.FileDinhKem.Count > 0)
                    {
                        var coQuanNgoaiTruong = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                        List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                        result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                        foreach (var item in Data.FileDinhKem)
                        {
                            if (item.CoQuanID != coQuanNgoaiTruong)
                            {
                                foreach (var cb in result)
                                {
                                    if (cb.CoQuanID == item.CoQuanID && cb.CanBoID == item.NguoiTaoID)
                                    {
                                        item.TenNguoiTao = cb.TenCanBo;
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
        [Route("DeTai_ThemMoi")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Create)]
        public async Task<IActionResult> DeTai_ThemMoi(IList<IFormFile> files, [FromForm] string ThongTinDeTai)
        {
            try
            {
                var CanBoID = 0;
                var CoQuanID = 0;
                try
                {
                    CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                }
                catch (Exception) { }
                var DeTaiModel = JsonConvert.DeserializeObject<DeTaiModel>(ThongTinDeTai);
                DeTaiModel.NguoiTaoID = CanBoID;
                var Result = _DeTaiBUS.Insert(DeTaiModel);
                int DeTaiID = Utils.ConvertToInt32(Result.Data, 0);
                //insert file đính kèm
                if (DeTaiID > 0 && files != null && files.Count > 0)
                {
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
                                            fileDinhKem.LoaiFile = EnumLoaiFileDinhKem.DeTai.GetHashCode();
                                            fileDinhKem.NguoiTaoID = CanBoID;
                                            fileDinhKem.CoQuanID = CoQuanID;
                                            fileDinhKem.FolderPath = nameof(EnumLoaiFileDinhKem.DeTai);
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
        [Route("DeTai_CapNhatTrangThai")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Edit)]
        public IActionResult DeTai_CapNhatTrangThai(DeTaiModel DeTaiModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _DeTaiBUS.Update_TrangThaiDeTai(DeTaiModel);
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
        [Route("DeTai_CapNhat")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Edit)]
        public IActionResult DeTai_CapNhat(DeTaiModel DeTaiModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    int CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    DeTaiModel.NguoiTaoID = CanBoID;
                    var Result = _DeTaiBUS.Update(DeTaiModel);
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
        [Route("DeTai_ChinhSuaThongTinChiTiet")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Edit)]
        public IActionResult DeTai_ChinhSuaThongTinChiTiet(ThongTinChiTietDeTaiModel ChiTiet)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    ChiTiet.NguoiTaoID = CanBoID;
                    ChiTiet.CoQuanID = CoQuanID;
                    var Result = _DeTaiBUS.Edit_ThongTinChiTiet(ChiTiet);
                    if (Utils.ConvertToInt32(Result.Data, 0) > 0)
                    {
                        if (ChiTiet.LoaiThongTin == EnumLoaiThongTin.KetQuaDanhGia.GetHashCode())
                        {
                            DeTaiModel dt = new DeTaiModel();
                            dt.DeTaiID = ChiTiet.DeTaiID;
                            if((ChiTiet.LoaiKetQua == 0 || ChiTiet.LoaiKetQua == null) && ChiTiet.LoaiNghiemThu != null && ChiTiet.NgayNghiemThu != null)
                            {
                                dt.TrangThai = EnumTrangThaiDeTai.NghiemThu.GetHashCode();
                            }
                            else if(ChiTiet.LoaiKetQua == 1)
                            {
                                dt.TrangThai = EnumTrangThaiDeTai.ThanhLy.GetHashCode();
                            }
                            
                            _DeTaiBUS.Update_TrangThaiDeTai(dt);
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
        [Route("DeTai_XoaThongTinChiTiet")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Delete)]
        public IActionResult DeTai_XoaThongTinChiTiet(ThongTinChiTietDeTaiModel ChiTiet)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _DeTaiBUS.Delete_ThongTinChiTiet(ChiTiet);
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
        [Route("DeTai_ChinhSuaKetQuaNghienCuu")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Edit)]
        public IActionResult DeTai_ChinhSuaKetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var CanBoID = 0;
                    try
                    {
                        CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    }
                    catch (Exception) { }
                    KetQuaNghienCuu.NguoiTaoID = CanBoID;
                    var Result = _DeTaiBUS.Edit_KetQuaNghienCuu(KetQuaNghienCuu);
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
        [Route("DeTai_XoaKetQuaNghienCuu")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Delete)]
        public IActionResult DeTai_XoaKetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _DeTaiBUS.Delete_KetQuaNghienCuu(KetQuaNghienCuu);
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

        [HttpGet]
        [Route("DeTai_ThongTinCanBo")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Read)]
        public IActionResult DeTai_ThongTinCanBo(string MaCanBo)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    HeThongCanBoModel CanBo = new HeThongCanBoModel();
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    if (result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            if (item.MaCB == MaCanBo)
                            {
                                CanBo = item;
                            }
                        }
                    }
                    if (CanBo.CanBoID > 0)
                    {
                        var chucDanhs = rsApiInCore.core_getTitles();
                        foreach (var item in chucDanhs)
                        {
                            if (item.Id == CanBo.ChucDanhID)
                            {
                                CanBo.TenChucDanh = item.Name;
                                break;
                            }
                        }

                        var hocHamHocVis = rsApiInCore.core_getDegrees();
                        foreach (var item in hocHamHocVis)
                        {
                            if (item.Id == CanBo.HocHamHocViID)
                            {
                                CanBo.TenHocHamHocVi = item.Name;
                                break;
                            }
                        }

                        var thongTinChiTiet = rsApiInCore.core_getstaff(CanBo.CanBoID);
                        CanBo.NgaySinh = thongTinChiTiet.Birthday;
                        base.Data = CanBo;
                    }

                    base.Status = 1;
                    base.TotalRow = TotalRow;

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
        [Route("DeTai_XoaDeTai")]
        [CustomAuthAttribute(ChucNangEnum.ql_de_tai, AccessLevel.Delete)]
        public IActionResult DeTai_XoaDeTai(DeTaiModel DeTaiModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _DeTaiBUS.Delete(DeTaiModel);
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



        [HttpGet]
        [Route("DuLieuNghienCuuKhoaHoc_Old")]
        public IActionResult DuLieuNghienCuuKhoaHoc_Old(int? StaffId, int? DepartmentId)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<DuLieuNghienCuuKhoaHocModel> Data = new List<DuLieuNghienCuuKhoaHocModel>();
                    List<int> listCanBoID = new List<int>();
                    if (StaffId == null || StaffId == 0)
                    {
                        var dsCanBo = rsApiInCore.core_DSCanBoTheoDonVi(DepartmentId ?? 0);
                        listCanBoID = dsCanBo.Select(x => x.Id).ToList();
                    }
                    else listCanBoID.Add(StaffId ?? 0);
                    Data = _DeTaiBUS.DuLieuNghienCuuKhoaHoc(listCanBoID, 0);
                    var clsCommon = new Commons();
                    var listNhiemVu = this.rsApiInCore.core_getConversions(0);
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    foreach (var item in Data)
                    {
                        if (item.Attach != null && item.Attach.Count > 0)
                        {
                            for (int i = 0; i < item.Attach.Count; i++)
                            {
                                item.Attach[i] = serverPath + item.Attach[i];
                            }
                        }

                        if (item.Id == 0)
                        {
                            foreach (var nv in listNhiemVu)
                            {
                                if (nv.Id == item.ConversionId) item.Id = nv.CategoryId;
                            }
                        }
                    }
                    base.Status = 1;
                    base.TotalRow = Data.Count;
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

        /// <summary>
        /// trả về dữ liệu nghiên cứu khoa học cho ht quản lý giờ giảng, 
        /// lấy danh sách đề tài, thông tin khai trong lý lịch khoa học (bài báo, sách)
        /// </summary>
        /// <param name="StaffId"></param>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DuLieuNghienCuuKhoaHoc")]
        public IActionResult DuLieuNghienCuuKhoaHoc(int? StaffId, int? DepartmentId, int? Year)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<DuLieuNghienCuuKhoaHocModel> Data = new List<DuLieuNghienCuuKhoaHocModel>();
                    List<int> listCanBoID = new List<int>();
                    var dsCanBo = rsApiInCore.core_DSCanBoTheoDonVi(DepartmentId ?? 0);
                    if (StaffId == null || StaffId == 0)
                    {
                        listCanBoID = dsCanBo.Select(x => x.Id).ToList();
                    }
                    else listCanBoID.Add(StaffId ?? 0);
                    Data = _DeTaiBUS.DuLieuNghienCuuKhoaHoc(listCanBoID, Year);
                    var clsCommon = new Commons();
                    var listNhiemVu = this.rsApiInCore.core_getConversions(0);
                    string serverPath = clsCommon.GetServerPath(HttpContext);

                    var ChiTietNhaKhoaHoc = _DeTaiBUS.DuLieuHoatDongKhoHocKhac(listCanBoID, Year);
                    List<DuLieuNghienCuuKhoaHocModel> listDuLieuNghienCuuKhac = new List<DuLieuNghienCuuKhoaHocModel>();
                    listDuLieuNghienCuuKhac = ChiTietNhaKhoaHoc.GroupBy(p => new { p.CTNhaKhoaHocID })
                            .Select(g => new DuLieuNghienCuuKhoaHocModel
                            {
                                Id = g.FirstOrDefault().LoaiNhiemVu.Value,
                                StaffId = g.FirstOrDefault().CanBoID,
                                ConversionId = g.FirstOrDefault().NhiemVu.Value,
                                Name = g.FirstOrDefault().TieuDe,
                                Quantity = g.FirstOrDefault().CTNhaKhoaHocID.Value,// lưu tạm thời
                                Members = g.Count(x => x.TacGiaID != null),
                                StartYear = g.FirstOrDefault().NamDangTai.Value,
                                Desc = g.FirstOrDefault().TieuDe
                                    + ((g.FirstOrDefault().TenTapChiSachHoiThao != null && g.FirstOrDefault().TenTapChiSachHoiThao.Length > 0) ? (", " + g.FirstOrDefault().TenTapChiSachHoiThao) : "")
                                    + ((g.FirstOrDefault().NhaXuatBan != null && g.FirstOrDefault().NhaXuatBan.Length > 0) ? (", " + g.FirstOrDefault().NhaXuatBan) : "")
                                    + ((g.FirstOrDefault().So != null && g.FirstOrDefault().So.Length > 0) ? (", " + g.FirstOrDefault().So) : "")
                                    + ((g.FirstOrDefault().Trang != null && g.FirstOrDefault().Trang.Length > 0) ? (", " + g.FirstOrDefault().Trang) : "")
                                    + ((g.FirstOrDefault().TenHocVien != null && g.FirstOrDefault().TenHocVien.Length > 0) ? (", " + g.FirstOrDefault().TenHocVien) : "")
                                    + ((g.FirstOrDefault().NguoiHuongDan != null && g.FirstOrDefault().NguoiHuongDan.Length > 0) ? (", " + g.FirstOrDefault().NguoiHuongDan) : "")
                                    + ((g.FirstOrDefault().LinkBaiBao != null && g.FirstOrDefault().LinkBaiBao.Length > 0) ? (", " + g.FirstOrDefault().LinkBaiBao) : "")
                                ,
                                WorkTime = 0,
                                Attach = ChiTietNhaKhoaHoc.Where(x => x.FileUrl != null && x.FileUrl.Length > 0).Select(y => y.FileUrl).ToList()
                            }
                            ).ToList();


                    if (listDuLieuNghienCuuKhac != null && listDuLieuNghienCuuKhac.Count > 0)
                    {
                        foreach (var item in listDuLieuNghienCuuKhac)
                        {
                            var dsTacGia = ChiTietNhaKhoaHoc.Where(x => x.CTNhaKhoaHocID == item.Quantity && x.TacGiaID != null && x.CoQuanTGID != null).ToList();
                            if (dsTacGia != null && dsTacGia.Count > 0)
                            {
                                var tgStr = "";
                                foreach (var i in dsTacGia)
                                {
                                    var tg = dsCanBo.FirstOrDefault(x => x.Id == i.TacGiaID && x.DepartmentId == i.CoQuanTGID);
                                    if (tg != null)
                                    {
                                        if (tgStr == "") tgStr = tg.Name;
                                        else tgStr = tgStr + ", " + tg.Name;
                                    }
                                }
                                item.Desc = item.StartYear.ToString() + (tgStr != "" ? (", " + tgStr) : "") + ", " + item.Desc;
                            }
                            else
                                item.Desc = item.StartYear.ToString() + ", " + item.Desc;
                            item.Quantity = 1;
                        }
                        Data.AddRange(listDuLieuNghienCuuKhac);
                    }
                    foreach (var item in Data)
                    {
                        if (item.Attach != null && item.Attach.Count > 0)
                        {
                            for (int i = 0; i < item.Attach.Count; i++)
                            {
                                item.Attach[i] = serverPath + item.Attach[i];
                            }
                        }

                        if (item.Id == 0)
                        {
                            foreach (var nv in listNhiemVu)
                            {
                                if (nv.Id == item.ConversionId) item.Id = nv.CategoryId;
                            }
                        }
                    }
                    base.Status = 1;
                    base.TotalRow = Data.Count;
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

        [HttpGet]
        [Route("GetDeTaiByCanBoID")]
        public IActionResult GetDeTaiByCanBoID(int CanBoID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<DeTaiModel> Data = new List<DeTaiModel>();
                    Data = _DeTaiBUS.GetDeTaiByCanBoID(CanBoID);
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