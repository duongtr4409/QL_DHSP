using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
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

namespace Com.Gosol.QLKH.API.Controllers.QLKH
{
    [Route("api/v1/BaoCaoThongKe")]
    [ApiController]
    public class BaoCaoThongKeController : BaseApiController
    {
        private IBaoCaoThongKeBUS _BaoCaoThongKeBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;
        private IMemoryCache _cache;

        public BaoCaoThongKeController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, IBaoCaoThongKeBUS BaoCaoThongKeBUS, IFileDinhKemBUS FileDinhKemBUS, ILogHelper _LogHelper, ILogger<DeTaiController> logger) : base(_LogHelper, logger)
        {
            this._BaoCaoThongKeBUS = BaoCaoThongKeBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
            _cache = memoryCache;
        }

        [HttpGet]
        [Route("BCThongKeNhiemVuKhoaHoc")]
        //[CustomAuthAttribute(ChucNangEnum.bao_cao, AccessLevel.Read)]
        public IActionResult BCThongKeNhiemVuKhoaHoc([FromQuery] BaoCaoThongKeParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    var dsCanBo = rsApiInCore.core_DSCanBoTheoDonVi(0);
                    dsCanBo = dsCanBo.Where(x => x.Gender == "Nữ").ToList();
                    List<int> listCanBoNuID = new List<int>();
                    if (dsCanBo != null && dsCanBo.Count > 0)
                    {
                        listCanBoNuID = dsCanBo.Select(x => x.Id).ToList();
                    }
                    List<BaoCaoThongKeModel> Data = _BaoCaoThongKeBUS.BCThongKeNhiemVuKhoaHoc(p, getStaffIDFromParams(p), listCanBoNuID); 
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
        [Route("BCDanhSachNhiemVuKhoaHoc")]
        //[CustomAuthAttribute(ChucNangEnum.bao_cao, AccessLevel.Read)]
        public IActionResult BCDanhSachNhiemVuKhoaHoc([FromQuery] BaoCaoThongKeParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<BCDanhSachNhiemVuKHModel> Data = _BaoCaoThongKeBUS.BCDanhSachNhiemVuKhoaHoc(p, getStaffIDFromParams(p));
                    //thêm thông tin tên chủ đề tài
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
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
        [Route("BCTinhHinhKetQuaThucHien")]
        //[CustomAuthAttribute(ChucNangEnum.bao_cao, AccessLevel.Read)]
        public IActionResult BCTinhHinhKetQuaThucHien([FromQuery] BaoCaoThongKeParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<BCTinhHinhKetQuaModel> Data = _BaoCaoThongKeBUS.BCTinhHinhKetQuaThucHien(p, getStaffIDFromParams(p));
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    foreach (var item in Data)
                    {
                        foreach (var nv in item.NhiemVuKhoaHoc)
                        {
                            if(nv.ChuNhiemDeTaiID > 0)
                            {
                                for (int i = 0; i < result.Count; i++)
                                {
                                    if (nv.ChuNhiemDeTaiID == result[i].CanBoID)
                                    {
                                        nv.DonViChuTri = result[i].TenCanBo;
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

        [HttpGet]
        [Route("BCThongKeHoatDongNghienCuu")]
        //[CustomAuthAttribute(ChucNangEnum.bao_cao, AccessLevel.Read)]
        public IActionResult BCThongKeHoatDongNghienCuu([FromQuery] BaoCaoThongKeParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<BCThongKeHoatDongNghienCuuModel> Data = _BaoCaoThongKeBUS.BCThongKeHoatDongNghienCuu(p, getStaffIDFromParams(p));
                    //thêm thông tin người chủ trì và các thành viên
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    if (result.Count > 0)
                    {
                        foreach (var item in Data)
                        {
                            if (item.NguoiChuTriIDStr != null && item.NguoiChuTriIDStr.Length > 0)
                            {
                                item.NguoiChuTri = "";
                                var listCanBoID = item.NguoiChuTriIDStr.Split(',');
                                if (listCanBoID.Length > 0)
                                {
                                    foreach (var CanBoID in listCanBoID)
                                    {
                                        for (int i = 0; i < result.Count; i++)
                                        {
                                            if (Utils.ConvertToInt32(CanBoID, 0) == result[i].CanBoID)
                                            {
                                                item.NguoiChuTri += result[i].TenCanBo + ", ";
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (item.NguoiChuTri.Length > 2) item.NguoiChuTri = item.NguoiChuTri.Substring(0, item.NguoiChuTri.Length - 2);
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

        [HttpGet]
        [Route("BCThongKeKetQuaNghienCuu")]
        //[CustomAuthAttribute(ChucNangEnum.bao_cao, AccessLevel.Read)]
        public IActionResult BCThongKeKetQuaNghienCuu([FromQuery] BaoCaoThongKeParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<BCThongKeKetQuaNghienCuuModel> Data = _BaoCaoThongKeBUS.BCThongKeKetQuaNghienCuu(p, getStaffIDFromParams(p));
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
        [Route("BCKeKhaiBaiBaoKhoaHoc")]
        //[CustomAuthAttribute(ChucNangEnum.bao_cao, AccessLevel.Read)]
        public IActionResult BCKeKhaiBaiBaoKhoaHoc([FromQuery] BaoCaoThongKeParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<BCKeKhaiBaiBaoKhoaHocModel> Data = _BaoCaoThongKeBUS.BCKeKhaiBaiBaoKhoaHoc(p);
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    List<DonViCoreModel> listDV = this.rsApiInCore.core_DSDonViTrucThuoc(0);

                    foreach (var item in Data)
                    {
                        //tac gia str
                        string ThanhVienThamGia = "";
                        if (item.TenCacTacGia != null || item.TenCacTacGia.Length > 0)
                        {
                            var listThanhVien = item.TenCacTacGia.Split(',');
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
                            if (ThanhVienThamGia.Length > 2) item.TenCacTacGia = ThanhVienThamGia.Substring(0, ThanhVienThamGia.Length - 2);
                        }

                        //Linh vuc nganh khoa hoc
                        if (item.LinhVucNganhKhoaHoc != null && item.LinhVucNganhKhoaHoc.Length > 0)
                        {
                            foreach (var dv in listDV)
                            {
                                if (item.LinhVucNganhKhoaHoc == dv.Id.ToString())
                                {
                                    item.LinhVucNganhKhoaHoc = dv.Name;
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
        [Route("CongBoKetQuaNghienCuu")]
        //[CustomAuthAttribute(ChucNangEnum.bao_cao, AccessLevel.Edit)]
        public IActionResult CongBoKetQuaNghienCuu(List<BCThongKeKetQuaNghienCuuModel> KetQuaNghienCuuModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _BaoCaoThongKeBUS.CongBoKetQuaNghienCuu(KetQuaNghienCuuModel);
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

        public List<int> getStaffIDFromParams(BaoCaoThongKeParams p)
        {
            var result = new List<int>();
            try
            {
                var dsCanBo = rsApiInCore.core_DSCanBoTheoDonVi(0);
                //var dsDanToc = rsApiInCore.core_DSCanBoTheoDonVi(0);
                var dsHocHam = rsApiInCore.core_getDegrees();
                var dsDonVi = rsApiInCore.core_DSDonViTrucThuoc(0);
                var dsChucVu = rsApiInCore.core_Positions();
                int NamSinh = p.NamSinh ?? 0;

                dsCanBo = dsCanBo.Where(x =>
                   (x.Code.ToLower().Contains((p.MaCanBo == null || p.MaCanBo == string.Empty || p.MaCanBo == "") ? x.Code.ToLower() : p.MaCanBo.ToLower()))
                   && (x.Name.ToLower().Contains((p.HoTen == null || p.HoTen == string.Empty || p.HoTen == "") ? x.Name.ToLower() : p.HoTen.ToLower()))
                   && (x.Gender.ToLower().Contains((p.GioiTinhStr == null || p.GioiTinhStr == string.Empty || p.GioiTinhStr == "") ? x.Gender.ToLower() : p.GioiTinhStr.ToLower()))
                   && (x.DegreeId == ((p.HocHamHocVi == null || p.HocHamHocVi == 0) ? x.DegreeId : p.HocHamHocVi.Value))
                   && (x.DepartmentId == ((p.DonViCongTac == null || p.DonViCongTac == 0) ? x.DepartmentId : p.DonViCongTac.Value))
                   && (p.ChucVu == null || p.ChucVu == 0 || x.PositionIds.Contains(p.ChucVu.Value))
                   && (p.NamSinh == null || (p.NamSinh != null && x.Birthday.ToString("yyyy") == NamSinh.ToString()))
                ).ToList();
                if (dsCanBo != null && dsCanBo.Count > 0)
                {
                    result = dsCanBo.Select(x => x.Id).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }
    }
}