using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
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
    [Route("api/v1/DonViNghienCuu")]
    [ApiController]
    public class DonViNghienCuuController : BaseApiController
    {
        private IHeThongCanBoBUS _HeThongCanBoBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;
        private IDeXuatDeTaiBUS _DeXuatDeTaiBUS;
        private ILyLichKhoaHocBUS _LyLichKhoaHocBUS;
        private IDeTaiBUS _DeTaiBUS;
        public DonViNghienCuuController(IDeTaiBUS DeTaiBUS, ILyLichKhoaHocBUS LyLichKhoaHocBUS, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, IHeThongCanBoBUS HeThongCanBoBUS, ILogHelper _LogHelper, ILogger<DeXuatDeTaiController> logger, IDeXuatDeTaiBUS DeXuatDeTaiBUS) : base(_LogHelper, logger)
        {
            this._HeThongCanBoBUS = HeThongCanBoBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
            this._DeXuatDeTaiBUS = DeXuatDeTaiBUS;
            this._LyLichKhoaHocBUS = LyLichKhoaHocBUS;
            this._DeTaiBUS = DeTaiBUS;
        }

        [HttpGet]
        [Route("DanhSachDonViNghienCuu")]
        public IActionResult DonViNghienCuu_DanhSach([FromQuery] BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<DonViCoreModel> listDataCore = this.rsApiInCore.core_DSDonViTrucThuoc(0);
                    List<DanhMucCoQuanDonViModel> listcq = new List<DanhMucCoQuanDonViModel>();

                    if (listDataCore.Count > 0)
                    {
                        foreach (var item in listDataCore)
                        {
                            DanhMucCoQuanDonViModel cq = new DanhMucCoQuanDonViModel();
                            cq.TenCoQuan = item.Name;
                            cq.CoQuanID = item.Id;
                            cq.Email = item.Email;
                            cq.DienThoai = item.Tel;
                            cq.DiaChi = item.Address;
                            //cq.NhanSuDonVi = getNhanSuDonViByCoQuanID(cq.CoQuanID).Count;
                            //cq.SoLuongDeXuatDaGui = getDanhSachDeXuatDaGuiFilter(cq.CoQuanID, null, null, null).Count;
                            listcq.Add(cq);
                        }
                    }
                    List<DanhMucCoQuanDonViModel> listcqfilter = new List<DanhMucCoQuanDonViModel>();
                    if (p.Keyword != null)
                    {
                        p.Keyword = p.Keyword.ToLower();
                        listcqfilter = listcq.Where(item => item.TenCoQuan.ToLower().Contains(p.Keyword.ToLower() ?? "")).ToList();
                    }
                    else
                    {
                        listcqfilter = listcq;
                    }
                    base.Status = 1;
                    base.TotalRow = listcqfilter.Count;
                    var data = listcqfilter.OrderBy(item => item.CoQuanID).Skip(p.Offset).Take(p.Limit).ToList();
                    var SoLuongBaiBaoVaSach = _LyLichKhoaHocBUS.SoLuongBaiBaoVaSach(DateTime.Now.Year);
                    var SoLuongDeTai = _DeTaiBUS.SoLuongDeTaiTheoCoQuan();
                    foreach (var item in data)
                    {
                        item.NhanSuDonVi = getNhanSuDonViByCoQuanID(item.CoQuanID).Count;
                        item.SoLuongDeXuatDaGui = getDanhSachDeXuatDaGuiFilter(item.CoQuanID, null, null, null, true).Count;
                        var BaiBao = SoLuongBaiBaoVaSach.FirstOrDefault(x => x.CoQuanID == item.CoQuanID);
                        if (BaiBao != null)
                        {
                            item.SoLuongBaiBao = BaiBao.SoLuongBaiBao;
                            item.SoLuongSach = BaiBao.SoLuongSach;
                        }
                        var detai = SoLuongDeTai.FirstOrDefault(x => x.CoQuanID == item.CoQuanID);
                        if (detai != null) item.SoLuongDeTai = detai.SoLuongDeTai;
                    }
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

        [HttpGet]
        [Route("DanhSachNhanSuTaiDonVi")]
        public IActionResult DeXuatDeTai_DanhSach([FromQuery] int CoQuanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_GetListPaging, EnumLogType.GetList, () =>
                {
                    //var listNguoiDungInCore = rsApiInCore.core_getusers(CoQuanID);
                    //var listInDB = new List<HeThongCanBoModel>();
                    //listNguoiDungInCore.ForEach(x => listInDB.Add(new HeThongCanBoModel()
                    //{
                    //    NguoiDungID = x.Id,
                    //    TenNguoiDung = x.Username,
                    //    CoQuanID = x.DepartmentId,
                    //    TenCanBo = x.Name,
                    //    CanBoID = x.StaffId,
                    //    Email = x.Email,
                    //    LaCanBoTrongTruong = true,
                    //}));
                    base.Status = 1;
                    base.Data = getNhanSuDonViByCoQuanID(CoQuanID, true).OrderBy(item => item.CanBoID).ToList();
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
        [Route("DanhSachDeXuatDaGuiFilter")]
        public IActionResult DanhSachDeXuatDaGuiFilter([FromQuery] int CoQuanID, int? CanBoID, int? LinhVucID, string? Keyword)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<DeXuatDeTaiModel> result = getDanhSachDeXuatDaGuiFilter(CoQuanID, CanBoID, LinhVucID, Keyword);
                    List<HeThongCanBoModel> listcanbo = getNhanSuDonViByCoQuanID(CoQuanID);
                    base.Status = 1;
                    base.Data = result;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Data = new List<DeXuatDeTaiModel>();
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <param name="CanBoID"></param>
        /// <param name="LinhVucID"></param>
        /// <param name="Keyword"></param>
        /// <param name="isCount">isCount = true -> Hàm dùng để đếm số lượng, false ->Lấy danh sách</param>
        /// <returns></returns>
        public List<DeXuatDeTaiModel> getDanhSachDeXuatDaGuiFilter(int CoQuanID, int? CanBoID, int? LinhVucID, string? Keyword, bool isCount = false)
        {
            List<DeXuatDeTaiModel> listDeXuat = _DeXuatDeTaiBUS.DanhSachDeXuatDaGuiFilter(CoQuanID, CanBoID ?? 0, LinhVucID ?? 0, Keyword ?? "", isCount);
            var clsCommon = new Commons();
            string serverPath = clsCommon.GetServerPath(HttpContext);
            if (!isCount)
            {
                List<HeThongCanBoModel> listcanbo = getNhanSuDonViByCoQuanID(CoQuanID);
                foreach (DeXuatDeTaiModel dexuat in listDeXuat)
                {
                    var canbo = listcanbo.FirstOrDefault(x => x.CanBoID == dexuat.NguoiDeXuat);
                    if (canbo != null)
                    {
                        dexuat.TenNguoiDeXuat = listcanbo.FirstOrDefault(x => x.CanBoID == dexuat.NguoiDeXuat).TenCanBo;
                    }
                }
            }
            foreach (var dexuat in listDeXuat)
            {
                if (dexuat.FileDinhKem != null)
                {
                    foreach (var file in dexuat.FileDinhKem)
                    {
                        file.FileUrl = serverPath + file.FileUrl;
                    }
                }
            }

            //List<DeXuatDeTaiModel> resultFilter = new List<DeXuatDeTaiModel>();
            //foreach (DeXuatDeTaiModel dexuat in listDeXuat)
            //{
            //    foreach (HeThongCanBoModel canbo in listcanbo)
            //    {
            //        if (dexuat.NguoiDeXuat == canbo.CanBoID)
            //        {
            //            dexuat.TenNguoiDeXuat = canbo.TenCanBo;
            //            resultFilter.Add(dexuat);
            //        }
            //    }
            //}
            return listDeXuat;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <param name="isCount">isCount = true -> Hàm dùng để đếm số lượng, false ->Lấy danh sách</param>
        /// <returns></returns>
        public List<HeThongCanBoModel> getNhanSuDonViByCoQuanID(int CoQuanID, bool isCount = false)
        {
            var listNguoiDungInCore = rsApiInCore.core_DSCanBoTheoDonVi(CoQuanID);
            var listChucVuInCore = rsApiInCore.core_Positions();
            var listInDB = new List<HeThongCanBoModel>();
            listNguoiDungInCore.ForEach(x => listInDB.Add(new HeThongCanBoModel()
            {
                NguoiDungID = x.Id,
                //TenNguoiDung = x.Username,
                CoQuanID = x.DepartmentId,
                TenCanBo = x.Name,
                CanBoID = x.Id,
                //Email = x.Email,
                LaCanBoTrongTruong = true,
                DanhSachChucVuID = x.PositionIds == null ? new List<int>() : x.PositionIds,
                //DanhSachTenChucVu = new List<string>(),
                DanhSachTenChucVu = listChucVuInCore.Where(i => x.PositionIds.Contains(i.Id)).ToList().Select(i => i.Name).ToList()
            })); ;
            //Lấy danh sách tên chức vụ theo ID
            //if (isCount)
            //{
            //    foreach (var item in listInDB)
            //    {
            //        foreach (var cvid in item.DanhSachChucVuID)
            //        {
            //            string tenchucvu = listChucVuInCore.FirstOrDefault(x => x.Id == cvid).Name;
            //            item.DanhSachTenChucVu.Add(tenchucvu);
            //        }
            //    }
            //}
            return listInDB;
        }


    }
}