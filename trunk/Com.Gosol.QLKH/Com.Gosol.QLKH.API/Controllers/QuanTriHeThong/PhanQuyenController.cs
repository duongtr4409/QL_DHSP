using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.Security;
using Microsoft.Extensions.Logging;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.API.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    /*
     * api/v1/PhanQuyen
     */
    [Route("api/v1/PhanQuyen")]
    [ApiController]
    public class PhanQuyenController : BaseApiController
    {
        private IPhanQuyenBUS _PhanQuyenBUS;
        private ISystemConfigBUS _SystemConfigBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        public PhanQuyenController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, IPhanQuyenBUS PhanQuyenBUS, ISystemConfigBUS _SystemConfigBUS, ILogHelper _LogHelper, ILogger<PhanQuyenController> logger) : base(_LogHelper, logger)
        {
            this._PhanQuyenBUS = PhanQuyenBUS;
            this._SystemConfigBUS = _SystemConfigBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
        }

        /// <summary>
        /// Lấy danh sách phân trang nhóm người dùng
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<NhomNguoiDungModel> Data;
                     var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                     var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                     //Data = _PhanQuyenBUS.NhomNguoiDung_GetPagingBySearch(p, CoQuanID, NguoiDungID, ref TotalRow);
                     Data = _PhanQuyenBUS.NhomNguoiDung_GetAll(p, CoQuanID, NguoiDungID, ref TotalRow);
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
        [Route("NhomNguoiDung_Insert")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Create)]
        public IActionResult Insert(NhomNguoiDungModel NhomNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    NhomNguoiDungModel.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    NhomNguoiDungModel.NhomTongID = 0;
                    var Result = _PhanQuyenBUS.NhomNguoiDung_Insert(NhomNguoiDungModel, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0), Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0));
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
        [Route("NhomNguoiDung_GetFoUpdate")]
        //[CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult NhomNguoiDung_GetFoUpdate(int NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetByID, EnumLogType.GetList, () =>
                {
                    var result = _PhanQuyenBUS.NhomNguoiDung_GetForUpdate(NhomNguoiDungID,
                            Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0),
                            Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0));
                    base.Status = result.Status;
                    base.Data = result.Data;
                    base.Message = result.Message;
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
        [Route("NhomNguoiDung_GetChiTietByNhomNguoiDungID")]
        //[CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult NhomNguoiDung_GetChiTietByNhomNguoiDungID(int NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetByID, EnumLogType.GetList, () =>
                {
                    var coQuanNgoaiTruong = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 0);
                    var Data = _PhanQuyenBUS.NhomNguoiDung_ChiTiet(NhomNguoiDungID);
                    var listNguoiDungInCore = rsApiInCore.core_getusers(0);
                    Data.DanhSachNguoiDung.Where(x => x.CoQuanID != coQuanNgoaiTruong).ToList()
                    .ForEach(x => x.TenNguoiDung = (listNguoiDungInCore.FirstOrDefault(i => i.Id == x.NguoiDungID /*&& i.DepartmentId == x.CoQuanID*/).Username ?? "") + " (" + (listNguoiDungInCore.FirstOrDefault(i => i.Id == x.NguoiDungID /*&& i.DepartmentId == x.CoQuanID*/).Name ?? "") + ")");
                    //Data.DanhSachNguoiDung.Where(x => x.CoQuanID == coQuanNgoaiTruong).ToList().ForEach(x => x.TenNguoiDung = x.TenNguoiDung + " (" + x.TenCanBo + ")");

                    base.Status = 1;
                    base.Data = Data;
                    base.Message = "";
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
        [Route("NhomNguoiDung_Delete")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Delete)]
        public IActionResult NhomNguioDung_Delete(DeleteModel NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Xoa, EnumLogType.Delete, () =>
                {
                    var Result = _PhanQuyenBUS.NhomNguoiDung_Delete(NhomNguoiDungID.NhomNguoiDungID.Value,
                        Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0),
                         Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0)
                        );
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
        [Route("NhomNguoiDung_Update")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Edit)]
        public IActionResult NhomNguoiDung_Update(NhomNguoiDungModel NhomNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _PhanQuyenBUS.NhomNguoiDung_Update(NhomNguoiDungModel, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0), Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0));
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
        [Route("NguoiDung_NhomNguoiDung_Insert")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Create)]
        public IActionResult NguoiDung_NhomNguoiDung_Insert(NguoiDungNhomNguoiDungModel NguoiDungNhomNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NguoiDung_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    var Result = _PhanQuyenBUS.NguoiDung_NhomNguoiDung_Insert(NguoiDungNhomNguoiDungModel);
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
        [Route("NguoiDung_NhomNguoiDung_Delete")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Delete)]
        public IActionResult NguoiDung_NhomNguoiDung_Delete(NguoiDungNhomNguoiDungModel NguoiDungNhomNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NguoiDung_NhomNguoiDung_Xoa, EnumLogType.Delete, () =>
                {
                    var Result = _PhanQuyenBUS.NguoiDung_NhomNguoiDung_Delete(NguoiDungNhomNguoiDungModel.NguoiDungID, NguoiDungNhomNguoiDungModel.NhomNguoiDungID, NguoiDungNhomNguoiDungModel.CoQuanID);
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
        [Route("PhanQuyen_Insert")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Create)]
        public IActionResult PhanQuyen_Insert(PhanQuyenModel PhanQuyenModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_PhanQuyen_Them, EnumLogType.Insert, () =>
                {
                    var Result = _PhanQuyenBUS.PhanQuyen_Insert(PhanQuyenModel);
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
        [Route("PhanQuyen_InsertMult")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Create)]
        public IActionResult PhanQuyen_InsertMult([FromBody] List<PhanQuyenModel> PhanQuyenModels)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_PhanQuyen_Them, EnumLogType.Insert, () =>
                {
                    var Result = _PhanQuyenBUS.PhanQuyen_InsertMulti(PhanQuyenModels);
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
        [Route("PhanQuyen_Delete")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Delete)]
        public IActionResult PhanQuyen_Delete(DeleteModel PhanQuyenID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_PhanQuyen_Xoa, EnumLogType.Delete, () =>
                {
                    var Result = _PhanQuyenBUS.PhanQuyen_Delete(PhanQuyenID.PhanQuyenID.Value);
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
        [Route("PhanQuyen_Update")]
        [CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Create)]
        public IActionResult PhanQuyen_Update(List<PhanQuyenModel> PhanQuyenModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_PhanQuyen_Sua, EnumLogType.Insert, () =>
                {
                    var Result = _PhanQuyenBUS.PhanQuyen_UpdateMulti(PhanQuyenModel);
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
        [Route("PhanQuyen_GetDanhSachCoQuan")]
        //[CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult PhanQuyen_GetDanhMucCoQuan()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var result = _PhanQuyenBUS.DanhMucCoQuan_GetAllFoPhanQuyen(Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0), Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0));
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu Cơ quan" : "";
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
        [Route("PhanQuyen_GetDanhSachCanBo")]
        //[CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult PhanQuyen_GetAllCanBoByListCoQuanID(int NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    var result = _PhanQuyenBUS.HeThongCanBo_GetAllByListCoQuanID(NhomNguoiDungID, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0), Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0));
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu cán bộ" : "";
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
        [Route("PhanQuyen_GetDanhSachNguoiDung")]
        //[CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult PhanQuyen_GetAllNguoiDungByListCoQuanID(int NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    var result = _PhanQuyenBUS.HeThongNguoiDung_GetAllByListCoQuanID(NhomNguoiDungID);
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu người dùng" : "";
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
        /// Danh sách quyền được thao tác trong nhóm
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PhanQuyen_GetQuyenDuocThaoTacTrongNhom")]
        //[CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult PhanQuyen_GetQuyenDuocThaoTacTrongNhom(int NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetByID, EnumLogType.GetList, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    if (_PhanQuyenBUS.CheckAdmin(NguoiDungID))
                        Data = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDungID);
                    else
                        Data = _PhanQuyenBUS.ChucNang_GetQuyenDuocThaoTacTrongNhom(NhomNguoiDungID, CoQuanID, NguoiDungID);
                    base.Status = 1;
                    //base.TotalRow = TotalRow;
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
        ///  danh sách nhóm người dùng cơ quan hiện tại được phân
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListNNDByCoQuanID")]
        //[CustomAuthAttribute(ChucNangEnum.phan_quyen, AccessLevel.Read)]
        public IActionResult GetListNhomNguoiDung_ByCoQuanID(int? CoQuanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<NhomNguoiDungModel> Data;
                    Data = _PhanQuyenBUS.GetListNhomNguoiDung_ByCoQuanID(CoQuanID);
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
        [Route("PhanQuyen_DanhSachCoQuan")]
        public IActionResult PhanQuyen_GetAllCoQuan()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    List<DanhMucCoQuanDonViModel> result = new List<DanhMucCoQuanDonViModel>();
                    result = _PhanQuyenBUS.DanhMucCoQuan_GetAllFoPhanQuyen(Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0), Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0));
                    //api lấy cơ quan trong trường
                    var CoQuanInCore = this.rsApiInCore.core_DSDonViTrucThuoc(0);
                    if (CoQuanInCore.Count > 0)
                    {
                        foreach (var item in CoQuanInCore)
                        {
                            DanhMucCoQuanDonViModel cq = new DanhMucCoQuanDonViModel();
                            cq.TenCoQuan = item.Name;
                            cq.CoQuanID = item.Id;
                            result.Add(cq);
                        }
                    }
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu cơ quan" : "";
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
        [Route("PhanQuyen_DSNguoiDungTheoCoQuan")]
        public IActionResult PhanQuyen_GetDSNguoiDungByCoQuanID(int? CoQuanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    List<HeThongNguoiDungModel> result = new List<HeThongNguoiDungModel>();
                    result = this.rsApiInCore.core_DSNguoiDungTheoCoQuan(CoQuanID ?? 0);
                    result.ForEach(x => x.TenNguoiDung = x.TenNguoiDung + " (" + x.TenCanBo + ")");
                    if (result.Count < 1)
                        result = _PhanQuyenBUS.GetAllNguoiDung();
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu người dùng" : "";
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
        [Route("PhanQuyen_DSNguoiDungTheoCoQuanVaNhomNguoiDungID")]
        public IActionResult PhanQuyen_GetDSNguoiDungByCoQuanID(int? CoQuanID, int NhomNguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    List<HeThongNguoiDungModel> result = new List<HeThongNguoiDungModel>();
                    var lstNguoiDungTrongNhom = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(NhomNguoiDungID);

                    result = this.rsApiInCore.core_DSNguoiDungTheoCoQuan(CoQuanID ?? 0).Where(x => !lstNguoiDungTrongNhom.Select(i => i.NguoiDungID).ToList().Contains(x.NguoiDungID)).ToList();
                    result.ForEach(x => x.TenNguoiDung = x.TenNguoiDung + " (" + x.TenCanBo + ")");
                    if (result.Count < 1)
                        result = _PhanQuyenBUS.GetAllNguoiDung().Where(x => !lstNguoiDungTrongNhom.Select(i => i.NguoiDungID).ToList().Contains(x.NguoiDungID)).ToList();
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu người dùng" : "";
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
        [Route("KhoiTaoQuyenNhaKhoaHoc")]
        public IActionResult KhoiTaoQuyenNhaKhoaHoc()
        {
            try
            {
                return CreateActionResult("Khởi tạo quyền nhà khoa học", EnumLogType.GetList, () =>
                {
                    var idNhomNguoiDungNKH = Utils.ConvertToInt32(_SystemConfigBUS.GetByKey("ID_NHOM_QUYEN_NKH").ConfigValue, 0);
                    var dsNguoiDungTrongNhomNKH = _PhanQuyenBUS.PhanQuyen_DanhSachNguoiDungTrongNhom(idNhomNguoiDungNKH);
                    var dsIDNguoiDungTrongNhomNKH = dsNguoiDungTrongNhomNKH.Select(x => x.NguoiDungID).ToList();
                    var dsNguoiDungTrongCore = rsApiInCore.core_getusers(0);
                    var dsIDNguoiDungTrongCore = dsNguoiDungTrongCore.Select(x => x.Id).ToList();
                    var dsNguoiDungChuaCoTrongNhomNKH = dsNguoiDungTrongCore.Where(x => !dsIDNguoiDungTrongNhomNKH.Contains(x.Id)).ToList();

                    if (dsNguoiDungChuaCoTrongNhomNKH != null && dsNguoiDungChuaCoTrongNhomNKH.Count > 0)
                    {
                        foreach (var item in dsNguoiDungChuaCoTrongNhomNKH)
                        {
                            var isr = new NguoiDungNhomNguoiDungModel();
                            isr.NguoiDungID = item.Id;
                            isr.CoQuanID = item.DepartmentId;
                            isr.NhomNguoiDungID = idNhomNguoiDungNKH;
                            var result = _PhanQuyenBUS.NguoiDung_NhomNguoiDung_Insert(isr);
                        }
                    }
                    base.Status = 1;
                    base.Data = Data;
                    base.Message = "OK";
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
    public class DeleteModel
    {
        public int? NhomNguoiDungID { get; set; }
        public int? PhanQuyenID { get; set; }
    }

}