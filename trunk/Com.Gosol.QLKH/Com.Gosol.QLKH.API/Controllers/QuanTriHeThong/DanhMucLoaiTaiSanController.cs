using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.DanhMuc;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.QLKH.API.Formats;
using static Com.Gosol.QLKH.BUS.DanhMuc.DanhMucLoaiTaiSanBUS;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.API.Authorization;
using Microsoft.Extensions.Logging;

namespace Com.Gosol.QLKH.API.Controllers.DanhMuc
{
    [Route("api/v1/DanhMucLoaiTaiSan")]
    [ApiController]
    public class DanhMucLoaiTaiSanController : BaseApiController
    {
        string TenLoaiTaiSan = "Loại Tài Sản";
        private IDanhMucLoaiTaiSanBUS _DanhMucLoaiTaiSanBUS;
        public DanhMucLoaiTaiSanController(IDanhMucLoaiTaiSanBUS DanhMucLoaiTaiSanBUS, ILogHelper _LogHelper, ILogger<DanhMucLoaiTaiSanController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucLoaiTaiSanBUS = DanhMucLoaiTaiSanBUS;
        }
        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiTaiSan, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(DanhMucLoaiTaiSanModel DanhMucLoaiTaiSanModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LoaiTaiSan_ThemLoaiTaiSan, EnumLogType.Insert, () =>
                 {
                     string Message = null;
                     int val = 0;
                     val = _DanhMucLoaiTaiSanBUS.Insert(DanhMucLoaiTaiSanModel, ref Message);
                     base.Message = Message;
                     base.Status = val;
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
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiTaiSan, AccessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(DanhMucLoaiTaiSanModel DanhMucLoaiTaiSanModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LoaiTaiSan_SuaLoaiTaiSan, EnumLogType.Update, () =>
                 {
                     string Message = null;

                     int val = 0;
                     val = _DanhMucLoaiTaiSanBUS.Update(DanhMucLoaiTaiSanModel, ref Message);
                     base.Message = Message;
                     base.Status = val;
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
        //  [CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiTaiSan, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LoaiTaiSan_XoaLoaiTaiSan, EnumLogType.Delete, () =>
                 {
                     var Result = _DanhMucLoaiTaiSanBUS.Delete(p.ListID);
                     if (Result.Count > 0)
                     {
                         string Mes = "";
                         foreach (var item in Result)
                         {
                             Mes = string.Concat(Mes, item, ",");
                         };
                         Mes = string.Concat("Loại tài sản ", Mes, " đang được sử dụng. Không thể xóa!");
                         Mes = Mes.Remove(Mes.LastIndexOf(","), 1);
                         base.Message = Mes;
                         //base.Data = Result;
                         base.Status = 0;
                         return base.GetActionResult();
                     }
                     else
                     {
                         base.Message = "Xóa thành công!";
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
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiTaiSan, AccessLevel.Read)]
        [Route("GetByID")]
        public IActionResult GetByID(int LoaiTaiSanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LoaiTaiSan_GetByID, EnumLogType.GetByID, () =>
                 {
                     DanhMucLoaiTaiSanModel Data;
                     Data = _DanhMucLoaiTaiSanBUS.GetByID(LoaiTaiSanID);
                     if (Data.LoaiTaiSanID <= 0)
                     {
                         base.Message = ConstantLogMessage.API_Error_NotExist;
                     }
                     else
                     {
                         base.Message = "Tìm thấy loại tài sản có ID là " + Data.LoaiTaiSanID;
                     }
                     base.Status = Data.LoaiTaiSanID > 0 ? 1 : 0;
                     base.Data = Data;
                     return base.GetActionResult();

                 });
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
            }
        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiTaiSan, AccessLevel.Read)]
        [Route("GetListPaging")]
        public IActionResult GetPagingBySearch([FromQuery]BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LoaiTaiSan_GetListPaging, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<DanhMucLoaiTaiSanModel> Data;
                     Data = _DanhMucLoaiTaiSanBUS.GetPagingBySearch(p, ref TotalRow);
                     //int totalRow = Data.Count();
                     if (Data.Count == 0)
                     {
                         base.Status = 1;
                         base.Message = ConstantLogMessage.API_NoData;
                         return base.GetActionResult();
                     }
                     base.Status = Data.Count > 0 ? 1 : 0;
                     base.Data = Data;
                     base.TotalRow = TotalRow;
                     return base.GetActionResult();
                 });
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }
        }

    }
}