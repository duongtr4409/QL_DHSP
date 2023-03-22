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
    [Route("api/v1/HoiDong")]
    [ApiController]
    public class HoiDongController : BaseApiController
    {
        private IHoiDongBUS _HoiDongBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;

        public HoiDongController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, IHoiDongBUS HoiDongBUS, IFileDinhKemBUS FileDinhKemBUS, ILogHelper _LogHelper, ILogger<DeTaiController> logger) : base(_LogHelper, logger)
        {
            this._HoiDongBUS = HoiDongBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
        }

        [HttpGet]
        [Route("HoiDong_DanhSach")]
        [CustomAuthAttribute(ChucNangEnum.ql_hoi_dong, AccessLevel.Read)]
        public IActionResult HoiDong_DanhSach([FromQuery] BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<int> listCanBo = new List<int>();
                    List<HeThongCanBoModel> dmCanBo = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    if(p.Keyword != null && p.Keyword.Length > 0)
                    {
                        foreach (var item in dmCanBo)
                        {
                            //if (p.Keyword.Contains(item.TenCanBo))
                            if (item.TenCanBo.ToLower().Contains(p.Keyword.ToLower()))
                            {
                                listCanBo.Add(item.CanBoID);
                            }
                        }
                    }

                    int TotalRow = 0;
                    List<HoiDongModel> Data = _HoiDongBUS.GetPagingBySearch(p, ref TotalRow, listCanBo);
                    if(Data.Count > 0)
                    {
                        //lấy tên cán bộ và địa chỉ công tác      
                        List<DonViCoreModel> dmDonVi = this.rsApiInCore.core_DSDonViTrucThuoc(0);
                        if (dmCanBo.Count > 0)
                        {
                            foreach (var item in Data)
                            {
                                foreach (var thanhVien in item.ThanhVienHoiDong)
                                {
                                    foreach (var cb in dmCanBo)
                                    {
                                        if(thanhVien.CanBoID == cb.CanBoID)
                                        {
                                            thanhVien.TenCanBo = cb.TenCanBo;
                                            foreach (var dv in dmDonVi)
                                            {
                                                if (cb.CoQuanID == dv.Id)
                                                {
                                                    thanhVien.DonViCongTac = dv.Name;
                                                    break;
                                                }   
                                            }
                                            break;
                                        }
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
        [Route("HoiDong_ChiTiet")]
        [CustomAuthAttribute(ChucNangEnum.ql_hoi_dong, AccessLevel.Read)]
        public IActionResult NhaKhoaHoc_ChiTiet(int HoiDongID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    HoiDongModel Data = _HoiDongBUS.GetByID(HoiDongID); 
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
        [Route("HoiDong_ChinhSuaThongTinChiTiet")]
        [CustomAuthAttribute(ChucNangEnum.ql_hoi_dong, AccessLevel.Edit)]
        public IActionResult HoiDong_ChinhSuaThongTinChiTiet(HoiDongModel HoiDongModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _HoiDongBUS.Edit_HoiDong(HoiDongModel);
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
        [Route("HoiDong_XoaThongTinHoiDong")]
        [CustomAuthAttribute(ChucNangEnum.ql_hoi_dong, AccessLevel.Delete)]
        public IActionResult HoiDong_XoaThongTinChiTiet(HoiDongModel HoiDongModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _HoiDongBUS.Delete_HoiDong(HoiDongModel);
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
        [Route("HoiDong_LuuDanhSachDanhGia")]
        [CustomAuthAttribute(ChucNangEnum.ql_hoi_dong, AccessLevel.Edit)]
        public IActionResult HoiDong_LuuDanhSachDanhGia(List<DanhSachHoiDongDanhGiaModel> DSHoiDongDanhGiaModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _HoiDongBUS.Insert_DanhGiaHoiDong(DSHoiDongDanhGiaModel);
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
        [Route("HoiDong_DanhSachDanhGia")]
        [CustomAuthAttribute(ChucNangEnum.ql_hoi_dong, AccessLevel.Read)]
        public IActionResult HoiDong_DanhSachDanhGia(int? HoiDongID, string Keyword, int? CapQuanLy, int? CanBoID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<DanhSachHoiDongDanhGiaModel> Data = _HoiDongBUS.GetByHoiDongID(HoiDongID ?? 0, Keyword, CapQuanLy ?? 0, CanBoID ?? 0);
                    //thêm thông tin tên người đề xuất
                    List<HeThongCanBoModel> result = new List<HeThongCanBoModel>();
                    result = this.rsApiInCore.core_DSCBTheoDonVi(0);
                    if (result.Count > 0)
                    {
                        foreach (var item in Data)
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
    }
}