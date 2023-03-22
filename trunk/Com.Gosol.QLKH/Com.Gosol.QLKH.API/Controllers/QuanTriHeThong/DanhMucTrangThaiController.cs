using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.DanhMuc;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.Security;
using Microsoft.Extensions.Logging;

namespace Com.Gosol.QLKH.API.Controllers.DanhMuc
{
    [Route("api/v1/DanhMucTrangThai")]
    [ApiController]
    public class DanhMucTrangThaiController : BaseApiController
    {
        private IDanhMucTrangThaiBUS _DanhMucTrangThaiBUS;
        public DanhMucTrangThaiController(IDanhMucTrangThaiBUS DanhMucTrangThaiBUS, ILogHelper _LogHelper, ILogger<DanhMucTrangThaiController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucTrangThaiBUS = DanhMucTrangThaiBUS;
        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.dm_trang_thai, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(DanhMucTrangThaiModel DanhMucTrangThaiModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_TrangThai_Them, EnumLogType.Insert, () =>
                 {
                     var Result = _DanhMucTrangThaiBUS.Insert(DanhMucTrangThaiModel);
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
        [CustomAuthAttribute(ChucNangEnum.dm_trang_thai, AccessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(DanhMucTrangThaiModel DanhMucTrangThaiModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_TrangThai_Sua, EnumLogType.Update, () =>
                {
                    var Result = _DanhMucTrangThaiBUS.Update(DanhMucTrangThaiModel, Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "CoQuanID").Value, 0));
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
        [CustomAuthAttribute(ChucNangEnum.dm_trang_thai, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_TrangThai_Xoa, EnumLogType.Delete, () =>
                {
                    var Result = _DanhMucTrangThaiBUS.Delete(p.ListID);
                    base.Message = Result.Message;
                    base.Status = Result.Status;
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
        //[CustomAuthAttribute(ChucNangEnum.dm_trang_thai, AccessLevel.Read)]
        [Route("GetByID")]
        public IActionResult GetByID(int TrangThaiID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_TrangThai_GetByID, EnumLogType.GetByID, () =>
                 {
                     var data = new DanhMucTrangThaiModel();
                     data = _DanhMucTrangThaiBUS.GetByID(TrangThaiID);
                     if (data == null)
                     { base.Message = ConstantLogMessage.API_NoData; base.Status = 0; }
                     else { base.Message = " "; base.Status = 1; }
                     base.Data = data;
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
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.dm_trang_thai, AccessLevel.Read)]
        [Route("GetListPaging")]
        [HttpGet]
        public IActionResult GetListPaging([FromQuery]BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_TrangThai_GetListPaging, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<DanhMucTrangThaiModel> Data;
                     Data = _DanhMucTrangThaiBUS.GetPagingBySearch(p, ref TotalRow);
                     base.Status = 1;
                     base.TotalRow = TotalRow;
                     base.Data = Data;
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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_TrangThai_GetListPaging, EnumLogType.GetList, () =>
                {
                    IList<DanhMucTrangThaiModel> Data;
                    Data = _DanhMucTrangThaiBUS.GetAll();
                    base.Status = 1;
                    base.Data = Data;
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