using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QLKH;
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

namespace Com.Gosol.QLKH.API.Controllers.QLKH
{
    [Route("api/v1/QuanLyThongBao")]
    [ApiController]
    public class QuanLyThongBaoController : BaseApiController
    {
        private IQuanLyThongBaoBUS _QuanLyThongBaoBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;

        public QuanLyThongBaoController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, IQuanLyThongBaoBUS QuanLyThongBaoBUS, IFileDinhKemBUS FileDinhKemBUS, ILogHelper _LogHelper, ILogger<DeTaiController> logger) : base(_LogHelper, logger)
        {
            this._QuanLyThongBaoBUS = QuanLyThongBaoBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
        }

        [HttpGet]
        [Route("ThongBao_DanhSach")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Read)]
        public IActionResult ThongBao_DanhSach([FromQuery] BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    List<QuanLyThongBaoModel> Data = _QuanLyThongBaoBUS.GetPagingBySearch(p, ref TotalRow);

                    List<DonViCanBoModel> result = new List<DonViCanBoModel>();
                    result = this.rsApiInCore.GetALLCanBo();
                    if (Data.Count > 0 && result.Count > 0)
                    {
                        foreach (var item in Data)
                        {
                            List<string> listDoiTuongCoQuan = new List<string>();
                            List<string> listDoiTuongCanBo = new List<string>();
                            int count = 0;
                            foreach (var dv in result[0].Children)
                            {
                                Boolean checkDonVi = true;
                                foreach (var cb in dv.Children)
                                {
                                    Boolean check = false;
                                    foreach (var doiTuong in item.DoiTuongThongBao)
                                    {
                                        if (cb.Id == doiTuong.CanBoID)
                                        {
                                            check = true;
                                            listDoiTuongCanBo.Add(cb.Name);
                                        }
                                    }
                                    if (!check)
                                    {
                                        checkDonVi = false;
                                        //break;
                                    }
                                }
                                if (checkDonVi && dv.Children.Count > 0)
                                {
                                    listDoiTuongCoQuan.Add(dv.Name);
                                    listDoiTuongCanBo = new List<string>();
                                }
                                if (checkDonVi) count++;
                            }
                            if (count == result[0].Children.Count)
                            {
                                item.TenDoiTuongThongBao = result[0].Name;
                            }
                            else
                            {
                                foreach (var tenDonVi in listDoiTuongCoQuan)
                                {
                                    item.TenDoiTuongThongBao += tenDonVi + "; ";
                                }
                                foreach (var tenCanBo in listDoiTuongCanBo)
                                {
                                    item.TenDoiTuongThongBao += tenCanBo + "; ";
                                }
                                if (item.TenDoiTuongThongBao != null && item.TenDoiTuongThongBao.Length > 2)
                                {
                                    item.TenDoiTuongThongBao = item.TenDoiTuongThongBao.Substring(0, item.TenDoiTuongThongBao.Length - 2);
                                }
                            }
                        }
                    }

                    //them ten doi tuong
                    var listCB = this.rsApiInCore.core_DSCanBoTheoDonVi(0);
                    var listCoQuan = this.rsApiInCore.core_DSDonViTrucThuoc(0);
                    if (Data.Count > 0 && result.Count > 0)
                    {
                        foreach (var item in Data)
                        {
                            if(item.DoiTuongThongBao != null && item.DoiTuongThongBao.Count > 0)
                            {
                                foreach (var dt in item.DoiTuongThongBao)
                                {
                                    for (int i = 0; i < listCB.Count; i++)
                                    {
                                        if(dt.CanBoID == listCB[i].Id)
                                        {
                                            dt.TenCanBo = listCB[i].Name;
                                            dt.MaCanBo = listCB[i].Code;
                                            dt.CoQuanID = listCB[i].DepartmentId;
                                        }
                                    }

                                    for (int i = 0; i < listCoQuan.Count; i++)
                                    {
                                        if (dt.CoQuanID == listCoQuan[i].Id)
                                        {
                                            dt.TenCoQuan = listCoQuan[i].Name; 
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

        [HttpPost]
        [Route("ThongBao_ChinhSuaThongTinChiTiet")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Edit)]
        public IActionResult ThongBao_ChinhSuaThongTinChiTiet(QuanLyThongBaoModel ThongBaoModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    int CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    ThongBaoModel.CoQuanID = CoQuanID;
                    ThongBaoModel.CanBoID = CanBoID;
                    var Result = _QuanLyThongBaoBUS.Edit_ThongBao(ThongBaoModel);
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
        [Route("ThongBao_ChiTiet")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Read)]
        public IActionResult ThongBao_ChiTiet(int ThongBaoID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    QuanLyThongBaoModel Data = _QuanLyThongBaoBUS.GetByID(ThongBaoID);
                    if (Data.LichSuChinhSuaThongBao != null && Data.LichSuChinhSuaThongBao.Count > 0)
                    {
                        List<HeThongCanBoModel> listCB = new List<HeThongCanBoModel>();
                        listCB = this.rsApiInCore.core_DSCBTheoDonVi(0);
                        foreach (var item in Data.LichSuChinhSuaThongBao)
                        {
                            if (item.TenNguoiChinhSua == null || item.TenNguoiChinhSua == "")
                            {
                                for (int i = 0; i < listCB.Count; i++)
                                {
                                    if (item.CanBoID == listCB[i].CanBoID)
                                    {
                                        item.TenNguoiChinhSua = listCB[i].TenCanBo;
                                        break;
                                    }
                                }
                            }
                        }

                        List<DonViCanBoModel> result = new List<DonViCanBoModel>();
                        result = this.rsApiInCore.GetALLCanBo();
                        //Thêm tên đối tượng thông báo
                        if (Data.DoiTuongThongBao != null && Data.DoiTuongThongBao.Count > 0)
                        {
                            List<string> listDoiTuongCoQuan = new List<string>();
                            List<string> listDoiTuongCanBo = new List<string>();
                            int count = 0;
                            foreach (var dv in result[0].Children)
                            {
                                Boolean checkDonVi = true;
                                foreach (var cb in dv.Children)
                                {
                                    Boolean check = false;
                                    foreach (var doiTuong in Data.DoiTuongThongBao)
                                    {
                                        if (cb.Id == doiTuong.CanBoID)
                                        {
                                            check = true;
                                            listDoiTuongCanBo.Add(cb.Name);
                                        }
                                    }
                                    if (!check)
                                    {
                                        checkDonVi = false;
                                        //break;
                                    }
                                }
                                if (checkDonVi && dv.Children.Count > 0)
                                {
                                    listDoiTuongCoQuan.Add(dv.Name);
                                    listDoiTuongCanBo = new List<string>();
                                }
                                if (checkDonVi) count++;
                            }
                            if (count == result[0].Children.Count)
                            {
                                Data.TenDoiTuongThongBao = result[0].Name;
                            }
                            else
                            {
                                foreach (var tenDonVi in listDoiTuongCoQuan)
                                {
                                    Data.TenDoiTuongThongBao += tenDonVi + "; ";
                                }
                                foreach (var tenCanBo in listDoiTuongCanBo)
                                {
                                    Data.TenDoiTuongThongBao += tenCanBo + "; ";
                                }
                                if (Data.TenDoiTuongThongBao != null && Data.TenDoiTuongThongBao.Length > 2)
                                {
                                    Data.TenDoiTuongThongBao = Data.TenDoiTuongThongBao.Substring(0, Data.TenDoiTuongThongBao.Length - 2);
                                }
                            }
                        }
                        //Thêm tên đối tượng thông báo trong lịch sử
                        if (Data.LichSuChinhSuaThongBao.Count > 0 && result.Count > 0)
                        {
                            foreach (var item in Data.LichSuChinhSuaThongBao)
                            {
                                List<string> listDoiTuongCoQuan = new List<string>();
                                List<string> listDoiTuongCanBo = new List<string>();
                                int count = 0;
                                foreach (var dv in result[0].Children)
                                {
                                    Boolean checkDonVi = true;
                                    foreach (var cb in dv.Children)
                                    {
                                        Boolean check = false;
                                        foreach (var doiTuong in item.DoiTuongThongBao)
                                        {
                                            if (cb.Id == doiTuong.CanBoID)
                                            {
                                                check = true;
                                                listDoiTuongCanBo.Add(cb.Name);
                                            }
                                        }
                                        if (!check)
                                        {
                                            checkDonVi = false;
                                            //break;
                                        }
                                    }
                                    if (checkDonVi && dv.Children.Count > 0)
                                    {
                                        listDoiTuongCoQuan.Add(dv.Name);
                                        listDoiTuongCanBo = new List<string>();
                                    }
                                    if (checkDonVi) count++;
                                }
                                if (count == result[0].Children.Count)
                                {
                                    item.TenDoiTuongThongBao = result[0].Name;
                                }
                                else
                                {
                                    foreach (var tenDonVi in listDoiTuongCoQuan)
                                    {
                                        item.TenDoiTuongThongBao += tenDonVi + "; ";
                                    }
                                    foreach (var tenCanBo in listDoiTuongCanBo)
                                    {
                                        item.TenDoiTuongThongBao += tenCanBo + "; ";
                                    }
                                    if (item.TenDoiTuongThongBao != null && item.TenDoiTuongThongBao.Length > 2)
                                    {
                                        item.TenDoiTuongThongBao = item.TenDoiTuongThongBao.Substring(0, item.TenDoiTuongThongBao.Length - 2);
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
        [Route("ThongBao_XoaThongBao")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Delete)]
        public IActionResult HoiDong_XoaThongTinChiTiet(QuanLyThongBaoModel ThongBaoModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _QuanLyThongBaoBUS.Delete_ThongBao(ThongBaoModel);
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
        [Route("ThongBao_DanhSachHienThi")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Read)]
        public IActionResult ThongBao_DanhSachHienThi([FromQuery] BasePagingParams p, int? CanBoID, int? CoQuanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int CanBoIDLogin = 0;
                    int CoQuanIDLogin = 0;
                    try
                    {
                        CanBoIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                        CoQuanIDLogin = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    }
                    catch (Exception) { }
                    int TotalRow = 0;
                    List<ChiTietHienThiThongBaoModel> Data = _QuanLyThongBaoBUS.GetDSThongBaoHienThi(p, ref TotalRow, CanBoID ?? CanBoIDLogin, CoQuanID ?? CoQuanIDLogin);
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
        [Route("ThongBao_TatThongBao")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Edit)]
        public IActionResult ThongBao_TatThongBao(List<DoiTuongThongBaoModel> DoiTuongThongBao)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _QuanLyThongBaoBUS.Update_TrangThaiTatThongBao(DoiTuongThongBao);
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
        [Route("ThongBao_DanhSachCanBo")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Read)]
        public IActionResult ThongBao_DanhSachCanBo()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<DonViCanBoModel> result = new List<DonViCanBoModel>();
                    result = this.rsApiInCore.GetALLCanBo();

                    int TotalRow = result.Count;
                    base.Status = 1;
                    base.TotalRow = TotalRow;
                    base.Data = result;
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
        [Route("ThongBao_DanhSachCanBoTheoCapQuanLy")]
        //[CustomAuthAttribute(ChucNangEnum.quan_tri_he_thong, AccessLevel.Read)]
        public IActionResult ThongBao_DanhSachCanBoTheoCap(int? CapQuanLy, int? NamBatDau)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    var Result = _QuanLyThongBaoBUS.GetDS_DoiTuongThongBaoTheoCap(CapQuanLy ?? 0, NamBatDau ?? 0);
                    var dsCanBoInCore = rsApiInCore.core_DSCanBoTheoDonVi(0);
                    var dsCoQuan = rsApiInCore.core_DSDonViTrucThuoc(0);
                    foreach (var item in Result)
                    {
                        var crCanBo = dsCanBoInCore.FirstOrDefault(x => x.Id == item.CanBoID);
                        var crCoQuan = dsCoQuan.FirstOrDefault(x => x.Id == item.CoQuanID);
                        if (crCanBo != null)
                            item.TenCanBo = crCanBo.Name;
                        if (crCoQuan != null)
                            item.TenCoQuan = crCoQuan.Name;
                    }
                    base.Status = 1;
                    base.TotalRow = TotalRow;
                    base.Data = Result;
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