using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.DanhMuc;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Com.Gosol.QLKH.API.Controllers.DanhMuc
{
    [Route("api/v1/DanhMucBuocThucHien")]
    [ApiController]

    public class DanhMucBuocThucHieNController : BaseApiController
    {
        private IDanhMucBuocThucHienBUS _DanhMucBuocThucHienBUS;
        private IHostingEnvironment _host;
        public DanhMucBuocThucHieNController(IDanhMucBuocThucHienBUS DanhMucBuocThucHienBUS, IHostingEnvironment hostingEnvironment, ILogHelper _LogHelper, ILogger<DanhMucChucVuController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucBuocThucHienBUS = DanhMucBuocThucHienBUS;
            this._host = hostingEnvironment;
        }

        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.HT_DanhMucChucVu, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(DanhMucBuocThucHienModel DanhMucBuocThucHienModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BuocThucHien_ThemChucVu, EnumLogType.Insert, () =>
                {
                    var Result = _DanhMucBuocThucHienBUS.Insert(DanhMucBuocThucHienModel);
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
        //  [CustomAuthAttribute(ChucNangEnum.DanhMuc_ChucVu, AccessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(DanhMucBuocThucHienModel DanhMucBuocThucHienModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BuocThucHien_SuaChucVu, EnumLogType.Update, () =>
                {
                    var Result = _DanhMucBuocThucHienBUS.Update(DanhMucBuocThucHienModel);
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
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_ChucVu, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_ChucVu_XoaChucVu, EnumLogType.Delete, () =>
                {

                    var Result = _DanhMucBuocThucHienBUS.Delete(p.ListID);
                    if (Result.Count > 0)
                    {
                        string Mes = "";
                        foreach (var item in Result)
                        {
                            Mes = string.Concat(Mes, item, ",");
                        };
                        Mes = string.Concat("Bước thực hiện ", Mes, " đang được sử dụng. Không thể xóa!");
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
        [Route("GetByID")]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_ChucVu, AccessLevel.Read)]
        public IActionResult GetByID(int BuocThucHienID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BuocThucHien_GetByID, EnumLogType.GetByID, () =>
                {
                    var Data = _DanhMucBuocThucHienBUS.GetByID(BuocThucHienID);
                    if (Data != null && Data.BuocThucHienID > 0)
                    {
                        base.Message = ConstantLogMessage.API_Success;
                        base.Data = Data;
                    }
                    else
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                    }
                    base.Status = 1;

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
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_ChucVu, AccessLevel.Read)]
        [Route("GetListPaging")]
        public IActionResult GetListPaging([FromQuery]BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BuocThucHien_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<DanhMucBuocThucHienModel> Data;
                    Data = _DanhMucBuocThucHienBUS.GetPagingBySearch(p, ref TotalRow);
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count >= 0 ? 1 : 0;
                    base.TotalRow = TotalRow;
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

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_ChucVu, AccessLevel.Read)]
        [Route("GetAll")]
        public IActionResult GetAll([FromQuery]BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BuocThucHien_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<DanhMucBuocThucHienModel> Data;
                    Data = _DanhMucBuocThucHienBUS.GetAll();
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count >= 0 ? 1 : 0;
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
    }
}
